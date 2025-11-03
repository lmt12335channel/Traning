using DBContext.DB;
using DBContext.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Service.Workspace.Executes.TrainingService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Workspace.Executes.TrainingService
{
    public class TrainingCommand
    {
        private readonly TrainingDbContexts _context;

        // Bỏ EmployeeMany vì đã chuyển sang dùng UserId từ Session
        public TrainingCommand(TrainingDbContexts context)
        {
            _context = context;
        }

        /// <summary>
        /// Lưu kết quả khảo sát ban đầu của người dùng vào database.
        /// </summary>
        /// <param name="form">Dữ liệu từ form khảo sát.</param>
        /// <param name="userId">ID của người dùng từ Session.</param>
        /// <returns>True nếu lưu thành công, False nếu thất bại.</returns>
        public async Task<bool> SaveSurveyAsync(IFormCollection form, int userId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Bước 1: Xóa các survey cũ của nhân viên này để đảm bảo chỉ có survey mới nhất là hợp lệ
                var existingSurveys = _context.EmployeeSurveys.Where(s => s.EmployeeId == userId);
                if (await existingSurveys.AnyAsync())
                {
                    _context.EmployeeSurveys.RemoveRange(existingSurveys);
                    await _context.SaveChangesAsync(); // Áp dụng thay đổi xóa
                }

                // Bước 2: Tạo bản ghi survey mới
                var survey = new EmployeeSurveys
                {
                    EmployeeId = userId,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                };
                await _context.EmployeeSurveys.AddAsync(survey);
                await _context.SaveChangesAsync(); // Lưu để lấy SurveyId cho các chi tiết

                // Bước 3: Phân tích form và tạo các bản ghi chi tiết
                var allCourses = await _context.TrainingCourses.AsNoTracking().ToListAsync();
                var details = new List<EmployeeSurveyDetails>();

                foreach (var course in allCourses)
                {
                    for (int i = 1; i <= 5; i++)
                    {
                        // Tên input trong form có dạng: name="course_IT0013"
                        string fieldName = $"course_{course.CourseCode}{i}";
                        if (form.ContainsKey(fieldName))
                        {
                            details.Add(new EmployeeSurveyDetails
                            {
                                SurveyId = survey.Id,
                                CourseId = course.Id,
                                SurveyLevel = i
                            });
                        }
                    }
                }

                if (details.Any())
                {
                    await _context.EmployeeSurveyDetails.AddRangeAsync(details);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"SaveSurvey Error: {ex}"); // Nên thay bằng ILogger
                return false;
            }
        }

        /// <summary>
        /// Lưu lựa chọn cuối cùng của người dùng (bao gồm Provider) vào database.
        /// </summary>
        /// <param name="employeeId">ID của người dùng.</param>
        /// <param name="selections">Danh sách các lựa chọn từ form xác nhận.</param>
        /// <returns>True nếu lưu thành công, False nếu thất bại.</returns>
        public async Task<bool> SaveFinalSelectionsAsync(int employeeId, List<TrainingModel> selections)
        {
            try
            {
                // Tìm survey mới nhất của nhân viên để cập nhật
                var latestSurvey = await _context.EmployeeSurveys
                    .Where(s => s.EmployeeId == employeeId)
                    .OrderByDescending(s => s.UpdatedDate)
                    .FirstOrDefaultAsync();

                if (latestSurvey == null) return false; // Không tìm thấy survey để cập nhật

                // Lấy tất cả chi tiết của survey đó
                var detailsToUpdate = await _context.EmployeeSurveyDetails
                    .Where(d => d.SurveyId == latestSurvey.Id).ToListAsync();

                foreach (var selection in selections)
                {
                    // `selection.Id` trong TrainingModel chính là CourseId
                    var detail = detailsToUpdate.FirstOrDefault(d => d.CourseId == selection.Id);
                    if (detail != null)
                    {
                        // Cập nhật ProviderId mà người dùng đã chọn
                        detail.SelectedProviderId = selection.SelectedProviderId;
                    }
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SaveFinalSelections Error: {ex}"); // Nên thay bằng ILogger
                return false;
            }
        }
    }
}
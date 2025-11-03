using DBContext.DB;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Workspace.Executes.TrainingService
{
    public class TrainingMany
    {
        private readonly TrainingDbContexts _context;
        public TrainingMany(TrainingDbContexts context) { _context = context; }

        public List<TrainingModel> GetAllCourses()
        {
            var data = _context.TrainingCourses
                .AsNoTracking()
                .Include(x => x.Field)
                .Select(x => new TrainingModel
                {
                    Id = x.Id,
                    CourseCode = x.CourseCode,
                    CourseName = x.Name,
                    FieldName = x.Field != null ? x.Field.Name : "N/A",
                    DurationHours = x.DurationHours,
                    Providers = x.Providers.Select(p => p.TrainingProvider.Name).Distinct().ToList()
                })
                .OrderBy(x => x.FieldName)
                .ToList();

            // SỬA LỖI: Thiếu return statement
            return data;
        }

        // SỬA LỖI: Trả về Task<List<TrainingModel>>
        public async Task<List<TrainingModel>> GetEmployeeSurveyResultsAsync(int employeeId)
        {
            var latestSurvey = await _context.EmployeeSurveys
                .Where(s => s.EmployeeId == employeeId)
                .OrderByDescending(s => s.UpdatedDate).AsNoTracking().FirstOrDefaultAsync();

            if (latestSurvey == null) return new List<TrainingModel>();

            var surveyDetails = await _context.EmployeeSurveyDetails
                .Where(d => d.SurveyId == latestSurvey.Id)
                .AsNoTracking()
                .Include(d => d.Course).ThenInclude(c => c.Field)
                .Include(d => d.Course).ThenInclude(c => c.Providers).ThenInclude(cp => cp.TrainingProvider)
                .Select(d => new TrainingModel
                {
                    Id = d.CourseId,
                    SurveyLevel = d.SurveyLevel,
                    CourseCode = d.Course.CourseCode,
                    CourseName = d.Course.Name,
                    FieldName = d.Course.Field.Name,
                    AvailableProviders = d.Course.Providers.Select(p => new SelectListItem
                    {
                        Value = p.TrainingProvider.Id.ToString(),
                        Text = p.TrainingProvider.Name
                    }).ToList()
                }).ToListAsync();
            return surveyDetails;
        }

        // SỬA LỖI: Nhận và trả về Task<List<TrainingModel>>
        public async Task<List<TrainingModel>> ReloadSurveyResultsWithProvidersAsync(int employeeId, List<TrainingModel> currentModel)
        {
            var freshData = await GetEmployeeSurveyResultsAsync(employeeId);
            foreach (var item in currentModel)
            {
                var freshItem = freshData.FirstOrDefault(f => f.Id == item.Id);
                if (freshItem != null)
                {
                    item.AvailableProviders = freshItem.AvailableProviders;
                }
            }
            return currentModel;
        }
    }
}
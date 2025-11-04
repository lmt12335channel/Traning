using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Workspace.Executes.TrainingService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TrainingManagement.Controllers
{
    public class TrainingController : Controller
    {
        private readonly TrainingMany _trainingMany;
        private readonly TrainingCommand _trainingCommand;

        public TrainingController(TrainingMany trainingMany, TrainingCommand trainingCommand)
        {
            _trainingMany = trainingMany;
            _trainingCommand = trainingCommand;
        }

        /// <summary>
        /// Action 1: Hiển thị danh sách tất cả các khóa học.
        /// </summary>
        public IActionResult Index()
        {
            var courses = _trainingMany.GetAllCourses();
            return View(courses);
        }

        /// <summary>
        /// Action 2: (GET) Hiển thị form để người dùng thực hiện khảo sát.
        /// </summary>
        [HttpGet]
        public IActionResult SaveSurvey()
        {
            var courses = _trainingMany.GetAllCourses();
            return View(courses);
        }

        /// <summary>
        /// Action 3: (POST) Nhận và lưu kết quả khảo sát ban đầu.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> SaveSurvey(IFormCollection form)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (!int.TryParse(userIdStr, out int userId))
            {
                TempData["Error"] = "Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Account");
            }

            var result = await _trainingCommand.SaveSurveyAsync(form, userId);

            if (result)
            {
                TempData["Success"] = "Bạn đã hoàn thành khảo sát! Vui lòng xem lại và xác nhận đăng ký.";
                return RedirectToAction("SurveyResults");
            }

            TempData["Error"] = "Lưu khảo sát thất bại. Vui lòng thử lại.";
            var courses = _trainingMany.GetAllCourses();
            return View(courses);
        }

        /// <summary>
        /// Action 4: (GET) Hiển thị kết quả khảo sát và cho phép người dùng chọn Provider.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> SurveyResults()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (!int.TryParse(userIdStr, out int userId))
            {
                TempData["Error"] = "Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Account");
            }

            var results = await _trainingMany.GetEmployeeSurveyResultsAsync(userId);

            if (results == null || !results.Any())
            {
                TempData["Info"] = "Bạn chưa thực hiện khảo sát hoặc không có lựa chọn nào được ghi nhận.";
                return RedirectToAction("Index");
            }
            return View(results);
        }

        /// <summary>
        /// Action 5: (POST) Nhận và lưu lựa chọn cuối cùng (bao gồm Provider) của người dùng.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> SubmitProviderSelections(List<TrainingModel> model)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (!int.TryParse(userIdStr, out int userId))
            {
                TempData["Error"] = "Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Vui lòng chọn đơn vị đào tạo cho tất cả các khóa học.";
                // Rất quan trọng: Tải lại danh sách dropdown trước khi trả về View
                var reloadedModel = await _trainingMany.ReloadSurveyResultsWithProvidersAsync(userId, model);
                return View("SurveyResults", reloadedModel);
            }

            var success = await _trainingCommand.SaveFinalSelectionsAsync(userId, model);

            if (success)
            {
                TempData["Success"] = "Đăng ký của bạn đã được ghi nhận thành công.";
            }
            else
            {
                TempData["Error"] = "Có lỗi xảy ra khi lưu đăng ký. Vui lòng thử lại.";
            }
            return RedirectToAction("Index");
        }
    }
}
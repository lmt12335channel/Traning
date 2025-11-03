using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Executes.EmployeeService; // Namespace của EmployeeMany
using System.Threading.Tasks;
using TrainingManagement.Models; // Namespace của LoginViewModel

namespace TrainingManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly EmployeeMany _employeeService;

        public AccountController(EmployeeMany employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            // Nếu người dùng đã đăng nhập, chuyển hướng họ đến trang chính
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail")))
            {
                return RedirectToAction("Index", "Training");
            }
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Thêm bảo mật
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Gọi phương thức xác thực từ service
                var employee = await _employeeService.AuthenticateAsync(model.Email, model.Password);

                if (employee != null)
                {
                    // Đăng nhập thành công, lưu thông tin quan trọng vào Session
                    HttpContext.Session.SetString("UserId", employee.Id.ToString());
                    HttpContext.Session.SetString("UserEmail", employee.Email ?? "N/A");
                    HttpContext.Session.SetString("UserName", employee.Fullname ?? "User");
                    HttpContext.Session.SetString("UserJobTitle", employee.JobPositionName ?? "");

                    // Chuyển hướng đến trang danh sách khóa học
                    return RedirectToAction("Index", "Training");
                }

                // Nếu employee là null, nghĩa là đăng nhập thất bại
                ModelState.AddModelError(string.Empty, "Email hoặc mật khẩu không đúng.");
            }

            // Nếu model không hợp lệ hoặc đăng nhập thất bại, hiển thị lại form
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Xóa tất cả dữ liệu session
            return RedirectToAction("Login", "Account"); // Chuyển hướng về trang đăng nhập
        }
    }
}
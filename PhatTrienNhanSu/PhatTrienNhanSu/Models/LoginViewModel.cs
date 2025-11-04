// File: TrainingManagement/Models/LoginViewModel.cs
using System.ComponentModel.DataAnnotations;

// Đảm bảo namespace này khớp với tên project của bạn
namespace TrainingManagement.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ email.")]
        [EmailAddress(ErrorMessage = "Định dạng email không hợp lệ.")]
        [Display(Name = "Địa chỉ Email")] // Nhãn sẽ hiển thị trên form
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")] // Nhãn sẽ hiển thị trên form
        public string Password { get; set; }

        // Bạn có thể thêm thuộc tính này nếu muốn có chức năng "Ghi nhớ tôi"
        // [Display(Name = "Ghi nhớ tôi?")]
        // public bool RememberMe { get; set; }
    }
}
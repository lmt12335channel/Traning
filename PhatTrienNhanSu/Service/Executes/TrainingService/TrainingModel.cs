using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Service.Workspace.Executes.TrainingService
{
    public class TrainingModel
    {
        // Thuộc tính gốc
        public int Id { get; set; }
        public string? CourseCode { get; set; }
        public string? CourseName { get; set; }
        public string? FieldName { get; set; }
        public decimal? DurationHours { get; set; }
        public List<string> Providers { get; set; } = new List<string>();

        // Thuộc tính gộp từ SurveyResultItemModel
        public int? SurveyLevel { get; set; }
        public List<SelectListItem> AvailableProviders { get; set; } = new List<SelectListItem>();

        // Thêm Data Annotation để validation hoạt động trên form
        [Required(ErrorMessage = "Vui lòng chọn đơn vị đào tạo.")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn đơn vị đào tạo.")]
        public int? SelectedProviderId { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBContext.Entities
{
    [Table("EmployeeSurveyDetails")]
    public class EmployeeSurveyDetails
    {
        [Key]
        public int Id { get; set; }

        public int SurveyId { get; set; }

        // === SỬA LỖI TẠI ĐÂY ===
        [ForeignKey("Course")] // Trỏ đến navigation property 'Course' ở dưới
        [Column("CourseId")]   // Ánh xạ thuộc tính này với cột 'CourseId' trong SQL
        public int CourseId { get; set; }
        // ======================

        public int SurveyLevel { get; set; }

        public int? SelectedProviderId { get; set; }

        public virtual TrainingCourses Course { get; set; } = null!;
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBContext.Entities
{
    [Table("TrainingCourses")]
    public class TrainingCourses
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string CourseCode { get; set; }

        // === SỬA LỖI TẠI ĐÂY ===
        [Required]
        [Column("CourseName")] // Ánh xạ thuộc tính 'Name' với cột 'CourseName' trong SQL
        public required string Name { get; set; }
        // ======================

        public decimal? DurationHours { get; set; }

        [ForeignKey("Field")]
        public int FieldId { get; set; }
        public virtual TrainingFields Field { get; set; } = null!;

        public virtual ICollection<TrainingCourseProviders> Providers { get; set; } = new List<TrainingCourseProviders>();

        public int Status { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        // Một khóa học có thể xuất hiện trong nhiều chi tiết khảo sát
        public virtual ICollection<EmployeeSurveyDetails> SurveyDetails { get; set; } = new List<EmployeeSurveyDetails>();
    }
}
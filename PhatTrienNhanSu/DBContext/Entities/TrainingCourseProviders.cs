using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBContext.Entities
{
    [Table("TrainingCourseProviders")] // Tên bảng trong database
    public class TrainingCourseProviders
    {
        [Key]
        public int Id { get; set; }

        // --- MỐI QUAN HỆ VỚI TRAININGCOURSES ---
        // Tên thuộc tính nên là CourseId (nhất quán với các bảng khác)
        // Tên trong ForeignKey trỏ đến tên Navigation Property ở dưới (TrainingCourse)
        [ForeignKey("TrainingCourse")]
        public int CourseId { get; set; }
        public virtual TrainingCourses TrainingCourse { get; set; } = null!;


        // --- MỐI QUAN HỆ VỚI TRAININGPROVIDERS ---
        // Tên thuộc tính nên là ProviderId
        // Tên trong ForeignKey trỏ đến tên Navigation Property ở dưới (TrainingProvider)
        [ForeignKey("TrainingProvider")]
        public int ProviderId { get; set; }
        public virtual TrainingProviders TrainingProvider { get; set; } = null!;


        // --- CÁC THUỘC TÍNH BỔ SUNG ---
        public DateTime? ContractDate { get; set; }
        public decimal? Cost { get; set; }
        public string? Note { get; set; }

        public int Status { get; set; } = 1;
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
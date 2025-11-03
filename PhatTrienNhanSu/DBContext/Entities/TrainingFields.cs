using System;
using System.Collections.Generic; // Cần thiết cho ICollection
using System.ComponentModel.DataAnnotations;

namespace DBContext.Entities
{
    public class TrainingFields
    {
        [Key] // Đánh dấu Id là khóa chính
        public int Id { get; set; }

        [Required] // Đảm bảo Name không bao giờ null
        public required string Name { get; set; }

        public string? Description { get; set; }

        public string? Keyword { get; set; }

        public int Status { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        // ======================== PHẦN THÊM VÀO QUAN TRỌNG NHẤT ========================
        /// <summary>
        /// Navigation property đại diện cho mối quan hệ "một-nhiều".
        /// Một TrainingFields (lĩnh vực) có thể có nhiều TrainingCourses (khóa học).
        /// Thuộc tính này rất cần thiết để Entity Framework thực hiện các phép JOIN (Include).
        /// </summary>
        public virtual ICollection<TrainingCourses> Courses { get; set; } = new List<TrainingCourses>();
        // ==============================================================================
    }
}
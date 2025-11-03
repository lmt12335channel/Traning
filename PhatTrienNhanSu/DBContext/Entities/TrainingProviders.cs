using System;

namespace DBContext.Entities
{
    public class TrainingProviders
    {
        public int Id { get; set; }
        public required string ProviderCode { get; set; }
        public required string Name { get; set; }
        public bool IsInternal { get; set; }
        public string? Keyword { get; set; }
        public int Status { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        // ======================== PHẦN THÊM VÀO ĐỂ SỬA LỖI ========================
        /// <summary>
        /// Navigation property đại diện cho mối quan hệ "một-nhiều" với bảng trung gian.
        /// Một TrainingProviders (nhà cung cấp) có thể liên kết với nhiều bản ghi
        /// trong TrainingCourseProviders (tức là cung cấp nhiều khóa học).
        /// </summary>
        public virtual ICollection<TrainingCourseProviders> Courses { get; set; } = new List<TrainingCourseProviders>();
        // =========================================================================
    }
}

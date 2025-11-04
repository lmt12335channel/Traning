using DBContext.Entities;
using Microsoft.EntityFrameworkCore;

namespace DBContext.DB
{
    public class TrainingDbContexts : DbContext
    {
        public TrainingDbContexts(DbContextOptions<TrainingDbContexts> options)
            : base(options)
        {
        }

        public DbSet<TrainingFields> TrainingFields { get; set; }
        public DbSet<TrainingProviders> TrainingProviders { get; set; }
        public DbSet<TrainingCourses> TrainingCourses { get; set; }
        public DbSet<SurveyPeriods> SurveyPeriods { get; set; }
        public DbSet<EmployeeSurveys> EmployeeSurveys { get; set; }
        public DbSet<EmployeeSurveyDetails> EmployeeSurveyDetails { get; set; }
        public DbSet<TrainingCourseProviders> TrainingCourseProviders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình cho bảng TrainingCourseProviders
            modelBuilder.Entity<TrainingCourseProviders>(entity =>
            {
                entity.HasOne(cp => cp.TrainingCourse)
                      .WithMany(c => c.Providers) // Trỏ đến ICollection<TrainingCourseProviders> Providers
                      .HasForeignKey(cp => cp.CourseId); // Dùng cột CourseId

                entity.HasOne(cp => cp.TrainingProvider)
                      .WithMany(p => p.Courses) // Trỏ đến ICollection<TrainingCourseProviders> Courses
                      .HasForeignKey(cp => cp.ProviderId); // Dùng cột ProviderId
            });

            // ======================== PHẦN SỬA LỖI QUAN TRỌNG NHẤT ========================
            // Cấu hình tường minh cho bảng EmployeeSurveyDetails
            modelBuilder.Entity<EmployeeSurveyDetails>(entity =>
            {
                // Định nghĩa mối quan hệ: một TrainingCourses có nhiều EmployeeSurveyDetails
                entity.HasOne(d => d.Course) // Navigation property "Course" trong EmployeeSurveyDetails
                      .WithMany(c => c.SurveyDetails) // Navigation property "SurveyDetails" trong TrainingCourses
                      .HasForeignKey(d => d.CourseId) // Khóa ngoại là thuộc tính "CourseId"
                      .OnDelete(DeleteBehavior.Cascade); // Tùy chọn: Xóa chi tiết nếu khóa học bị xóa

                // (Không bắt buộc, nhưng nên có)
                // Ép EF Core phải sử dụng đúng tên cột "CourseId" trong database
                entity.Property(d => d.CourseId).HasColumnName("CourseId");
            });
            // ==============================================================================
        }
    }
}
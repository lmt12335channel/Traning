using System;

namespace DBContext.Entities
{
    public class EmployeeSurveys
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int SurveyPeriodId { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string? Keyword { get; set; }
        public int Status { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}

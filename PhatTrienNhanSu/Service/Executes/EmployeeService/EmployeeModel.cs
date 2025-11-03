using Newtonsoft.Json;
using System.Collections.Generic;

// Đảm bảo namespace này khớp với cấu trúc thư mục của bạn
namespace Service.Workspace.Executes.EmployeeService
{
    /// <summary>
    /// Đại diện cho thông tin cơ bản của một nhân viên.
    /// </summary>
    public class EmployeeModel
    {
        public int Id { get; set; }
        public string? Fullname { get; set; }
        public string? Email { get; set; }
        public string? JobPositionName { get; set; }
        // Thêm các thuộc tính khác nếu API trả về (ví dụ: Role, DepartmentName...)
    }

    // ======================== CÁC LỚP HELPER ĐƯỢC DI CHUYỂN VÀO ĐÂY ========================

    /// <summary>
    /// Đại diện cho cấu trúc JSON trả về từ API tìm kiếm (trả về một danh sách).
    /// </summary>
    public class EmployeeApiResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string? Message { get; set; }

        [JsonProperty("data")]
        public List<EmployeeModel> Data { get; set; } = new List<EmployeeModel>();
    }

    /// <summary>
    /// Đại diện cho cấu trúc JSON trả về từ API đăng nhập (chỉ trả về một nhân viên).
    /// </summary>
    public class SingleEmployeeApiResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string? Message { get; set; }

        [JsonProperty("data")]
        public EmployeeModel? Data { get; set; }
    }
}
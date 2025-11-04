using Newtonsoft.Json;
using Service.Workspace.Executes.EmployeeService;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Service.Executes.EmployeeService
{
    public class EmployeeMany
    {
        private readonly HttpClient _httpClient;

        // Định nghĩa địa chỉ cơ sở của API để dễ dàng quản lý
        private const string ApiBaseUrl = "http://192.168.1.86:5000";

        public EmployeeMany(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Phương thức này dùng để TÌM KIẾM NHÂN VIÊN, không dùng để đăng nhập.
        /// API này có thể có địa chỉ IP khác.
        /// </summary>
        public async Task<List<EmployeeModel>> GetEmployeeByEmailAsync(string email)
        {
            // Địa chỉ IP này khác với IP đăng nhập, giữ nguyên theo code gốc của bạn
            var searchApiUrl = $"http://192.168.1.130:5000/api/employee/email/{email}";
            var response = await _httpClient.GetAsync(searchApiUrl);

            if (!response.IsSuccessStatusCode)
            {
                return new List<EmployeeModel>();
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<EmployeeApiResponse>(json);
            return result?.Data ?? new List<EmployeeModel>();
        }

        /// <summary>
        /// Gửi email và mật khẩu đến API để xác thực đăng nhập.
        /// </summary>
        /// <param name="email">Email người dùng nhập.</param>
        /// <param name="password">Mật khẩu người dùng nhập.</param>
        /// <returns>Đối tượng EmployeeModel nếu đăng nhập thành công, ngược lại trả về null.</returns>
        public async Task<EmployeeModel> AuthenticateAsync(string email, string password)
        {
            // Endpoint để đăng nhập, ĐÚNG THEO YÊU CẦU CỦA BẠN
            var loginUrl = $"{ApiBaseUrl}/api/account/sign-in-view";

            // Tạo đối tượng chứa dữ liệu để gửi đi
            var loginRequest = new
            {
                Email = email,
                Password = password
            };

            try
            {
                // Gửi yêu cầu POST với dữ liệu loginRequest dưới dạng JSON
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync(loginUrl, loginRequest);

                // Nếu API trả về mã thành công (ví dụ: 200 OK)
                if (response.IsSuccessStatusCode)
                {
                    // Đọc nội dung JSON trả về và chuyển đổi nó thành đối tượng EmployeeModel
                    var json = await response.Content.ReadAsStringAsync();

                    // Sử dụng lớp helper để phân tích JSON trả về
                    var apiResponse = JsonConvert.DeserializeObject<SingleEmployeeApiResponse>(json);

                    // Trả về đối tượng nhân viên nếu có
                    return apiResponse?.Data;
                }

                // Nếu API trả về lỗi (ví dụ: 401 Unauthorized - sai mật khẩu, 404 Not Found - sai email)
                // thì trả về null để báo hiệu đăng nhập thất bại.
                return null;
            }
            catch (HttpRequestException ex)
            {
                // Xử lý các lỗi liên quan đến kết nối mạng (ví dụ: sai IP, API không chạy)
                // Bạn nên sử dụng ILogger ở đây để ghi lại lỗi chi tiết.
                System.Console.WriteLine($"Lỗi kết nối API đăng nhập tại {loginUrl}: {ex.Message}");
                return null;
            }
        }
    }
}
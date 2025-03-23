using EduFlow.BLL.DTOs.Courses.Attendance;
using EduFlow.Desktop.Integrated.Api.Auth;
using EduFlow.Desktop.Integrated.Security;
using EduFlow.Desktop.Integrated.Servers.Interfaces.Courses.Attendance;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace EduFlow.Desktop.Integrated.Servers.Repositories.Courses.Attendance;

public class AttendanceServer : IAttendanceServer
{
    public async Task<bool> AddAsync(AttendanceForCraeteDto dto)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/attendances");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress)
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(dto),
                    Encoding.UTF8,
                    "application/json")
            };

            var response = await client.SendAsync(request);

            if(response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<bool> DeleteAsync(long id)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;
            
            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/attendances/{id}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var request = new HttpRequestMessage(HttpMethod.Delete, client.BaseAddress);
            
            var response = await client.SendAsync(request);
            
            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<List<AttendanceForResultDto>> GetAllAsync()
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;
            
            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/attendances");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var request = new HttpRequestMessage(HttpMethod.Get, client.BaseAddress);
            
            var response = await client.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<List<AttendanceForResultDto>>(content)!;

            return result;
        }
        catch (Exception ex)
        {
            return new List<AttendanceForResultDto>();
        }
    }

    public async Task<List<AttendanceForResultDto>> GetAllByCourseIdAsync(long courseId)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/attendances/{courseId}/course");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = new HttpRequestMessage(HttpMethod.Get, client.BaseAddress);

            var response = await client.SendAsync(request);
            
            var content = await response.Content.ReadAsStringAsync();
            
            var result = JsonSerializer.Deserialize<List<AttendanceForResultDto>>(content)!;
            
            return result;
        }
        catch (Exception ex)
        {
            return new List<AttendanceForResultDto>();
        }
    }

    public async Task<List<AttendanceForResultDto>> GetAllByStudentIdAsync(long studentId)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;
            
            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/attendances/{studentId}/student");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var request = new HttpRequestMessage(HttpMethod.Get, client.BaseAddress);
            
            var response = await client.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<List<AttendanceForResultDto>>(content)!;

            return result;
        }
        catch (Exception ex)
        {
            return new List<AttendanceForResultDto>();
        }
    }

    public async Task<AttendanceForResultDto> GetByIdAsync(long id)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/attendances/{id}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = new HttpRequestMessage(HttpMethod.Get, client.BaseAddress);

            var response = await client.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<AttendanceForResultDto>(content)!;

            return result;
        }
        catch (Exception ex)
        {
            return new AttendanceForResultDto();
        }
    }

    public async Task<bool> UpdateAsync(long id, AttendanceForUpdateDto dto)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;
            
            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/attendances/{id}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var request = new HttpRequestMessage(HttpMethod.Put, client.BaseAddress)
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(dto),
                    Encoding.UTF8,
                    "application/json")
            };
            
            var response = await client.SendAsync(request);
            
            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}

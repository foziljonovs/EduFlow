using EduFlow.BLL.DTOs.Users.Student;
using EduFlow.BLL.DTOs.Users.Teacher;
using EduFlow.Desktop.Integrated.Security;
using EduFlow.Desktop.Integrated.Servers.Interfaces.Users.Student;
using Newtonsoft.Json;
using System.Data.Common;

namespace EduFlow.Desktop.Integrated.Servers.Repositories.Users.Student;

public class StudentServer : IStudentServer
{
    public Task<bool> AddAsync(TeacherForCreateDto dto)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/students");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress)
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(dto),
                    Encoding.UTF8,
                    "application/json")
            };

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }
        catch(Exception ex)
        {
            return false;
        }
    }

    public Task<bool> DeleteAsync(long id)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/students/{id}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.DeleteAsync(client.BaseAddress);

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

    public Task<List<TeacherForResultDto>> GetAllAsync()
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/students");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync(client.BaseAddress);

            var result = await response.Content.ReadAsStringAsync();

            List<StudentForResultDto> students = JsonConvert.DeserializeObject<List<StudentForResultDto>>(result)!;

            return students;
        }
        catch(Exception ex)
        {
            return new List<StudentForResultDto>();
        }
    }

    public Task<TeacherForResultDto> GetByIdAsync(long id)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/students/{id}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync(client.BaseAddress);

            var result = await response.Content.ReadAsStringAsync();

            StudentForResultDto student = JsonConvert.DeserializeObject<StudentForResultDto>(result)!;

            return student;
        }
        catch(Exception ex)
        {
            return new StudentForResultDto();
        }
    }

    public Task<bool> UpdateAsync(long id, TeacherForUpdateDto dto)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/students/{id}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = new HttpRequestMessage(HttpMethod.Put, client.BaseAddress)
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(dto),
                    Encoding.UTF8,
                    "application/json")
            };

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }
        catch(Exception ex)
        {
            return false;
        }
    }
}

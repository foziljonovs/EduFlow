using EduFlow.BLL.DTOs.Courses.Course;
using EduFlow.Desktop.Integrated.Api.Auth;
using EduFlow.Desktop.Integrated.Security;
using EduFlow.Desktop.Integrated.Servers.Interfaces.Courses.Course;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace EduFlow.Desktop.Integrated.Servers.Repositories.Courses.Course;

public class CourseServer : ICourseServer
{
    public async Task<bool> AddAsync(CourseForCreateDto dto)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/courses");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress)
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(dto),
                    Encoding.UTF8,
                    "application/json")
            };

            var response = await client.SendAsync(request);

            if(response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }
        catch(Exception ex)
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

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/courses/{id}");
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

    public async Task<List<CourseForResultDto>> FilterAsync(CourseForFilterDto dto)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/courses/filter");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress)
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(dto),
                    Encoding.UTF8,
                    "application/json")
            };

            var response = await client.SendAsync(request);

            var result = await response.Content.ReadAsStringAsync();

            List<CourseForResultDto> courses = JsonConvert.DeserializeObject<List<CourseForResultDto>>(result)!;

            return courses;
        }
        catch(Exception ex)
        {
            return new List<CourseForResultDto>();
        }
    }

    public async Task<List<CourseForResultDto>> GetAllAsync()
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/courses");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync(client.BaseAddress);
            
            var result = await response.Content.ReadAsStringAsync();

            List<CourseForResultDto> courses = JsonConvert.DeserializeObject<List<CourseForResultDto>>(result)!;

            return courses;
        }
        catch (Exception ex)
        {
            return new List<CourseForResultDto>();
        }
    }

    public async Task<List<CourseForResultDto>> GetAllByCategoryIdAsync(long categoryId)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/courses/{categoryId}/category");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync(client.BaseAddress);

            var result = await response.Content.ReadAsStringAsync();

            List<CourseForResultDto> courses = JsonConvert.DeserializeObject<List<CourseForResultDto>>(result)!;

            return courses;
        }
        catch (Exception ex)
        {
            return new List<CourseForResultDto>();
        }
    }

    public async Task<List<CourseForResultDto>> GetAllByTeacherIdAsync(long teacherId)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/courses/{teacherId}/teacher");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync(client.BaseAddress);

            var result = await response.Content.ReadAsStringAsync();
            
            List<CourseForResultDto> courses = JsonConvert.DeserializeObject<List<CourseForResultDto>>(result)!;
            
            return courses;
        }
        catch (Exception ex)
        {
            return new List<CourseForResultDto>();
        }
    }

    public async Task<CourseForResultDto> GetByIdAsync(long id)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/courses/{id}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync(client.BaseAddress);
            
            var result = await response.Content.ReadAsStringAsync();
            
            CourseForResultDto course = JsonConvert.DeserializeObject<CourseForResultDto>(result)!;
            
            return course;
        }
        catch (Exception ex)
        {
            return new CourseForResultDto();
        }
    }

    public async Task<bool> UpdateAsync(long id, CourseForUpdateDto dto)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/courses/{id}");
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
        catch (Exception ex)
        {
            return false;
        }
    }
}

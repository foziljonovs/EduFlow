using EduFlow.BLL.DTOs.Payments.Payment;
using EduFlow.BLL.DTOs.Users.Student;
using EduFlow.Desktop.Integrated.Api.Auth;
using EduFlow.Desktop.Integrated.Helpers;
using EduFlow.Desktop.Integrated.Security;
using EduFlow.Desktop.Integrated.Servers.Interfaces.Users.Student;
using EduFlow.Domain.Entities.Payments;
using EduFlow.Domain.Entities.Users;
using EduFlow.Domain.Enums;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Text;

namespace EduFlow.Desktop.Integrated.Servers.Repositories.Users.Student;

public class StudentServer : IStudentServer
{
    public async Task<long> AddAndReturnIdAsync(StudentForCreateDto dto)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/students/with-id");
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
            {
                var result = await response.Content.ReadAsStringAsync();
                long id = JsonConvert.DeserializeObject<long>(result)!;
                return id;
            }
            else
                return 0;
        }
        catch(Exception ex)
        {
            return 0;
        }
    }

    public async Task<bool> AddAsync(StudentForCreateDto dto)
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

    public async Task<bool> AddStudentByCourseAsync(long studentId, long courseId)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/students/{studentId}/courses/{courseId}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress);

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

    public async Task<bool> DeleteAsync(long id)
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

    public async Task<PagedResponse<StudentForResultDto>> FilterAsync(StudentForFilterDto dto, int pageSize, int pageNumber)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/students/filter?pageSize={pageSize}&pageNumber={pageNumber}");
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

            List<StudentForResultDto> students = JsonConvert.DeserializeObject<List<StudentForResultDto>>(result)!;

            var pageNavigation = response.Headers.GetValues("X-Pagination").FirstOrDefault();
            var pagination = JsonConvert.DeserializeObject<PaginationMetadata>(pageNavigation!);

            return new PagedResponse<StudentForResultDto>
            {
                Data = students,
                PageSize = pagination.PageSize,
                CurrentPage = pagination.CurrentPage,
                HasNext = pagination.HasNext,
                HasPrevious = pagination.HasPrevious
            };
        }
        catch(Exception ex)
        {
            return new PagedResponse<StudentForResultDto>();
        }
    }

    public async Task<List<StudentForResultDto>> GetAllAsync()
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

    public async Task<List<StudentForResultDto>> GetAllByCategoryIdAsync(long categoryId)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/students/{categoryId}/category");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync(client.BaseAddress);
            
            var result = await response.Content.ReadAsStringAsync();
            
            List<StudentForResultDto> students = JsonConvert.DeserializeObject<List<StudentForResultDto>>(result)!;
            
            return students;
        }
        catch (Exception ex)
        {
            return new List<StudentForResultDto>();
        }
    }

    public async Task<List<StudentForResultDto>> GetAllByCourseIdAsync(long courseId)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/students/{courseId}/course");
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

    public async Task<List<StudentForResultDto>> GetAllByTeacherIdAsync(long teacherId)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/students/{teacherId}/teacher");
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

    public async Task<PagedResponse<StudentForResultDto>> GetAllPaginationAsync(int pageSize, int pageNumber)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/students?pageSize={pageSize}&pageNumber={pageNumber}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync(client.BaseAddress);

            var result = await response.Content.ReadAsStringAsync();

            List<StudentForResultDto> students = JsonConvert.DeserializeObject<List<StudentForResultDto>>(result)!;

            var pageNavigation = response.Headers.GetValues("X-Pagination").FirstOrDefault();
            var pagination = JsonConvert.DeserializeObject<PaginationMetadata>(pageNavigation!);

            return new PagedResponse<StudentForResultDto>
            {
                Data = students,
                PageSize = pagination.PageSize,
                CurrentPage = pagination.CurrentPage,
                HasNext = pagination.HasNext,
                HasPrevious = pagination.HasPrevious
            };
        }
        catch(Exception ex)
        {
            return new PagedResponse<StudentForResultDto>();
        }
    }

    public async Task<StudentForResultDto> GetByIdAsync(long id)
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

    public async Task<PagedResponse<StudentForResultDto>> GetByPhoneNumberAsync(string phoneNumberSuffix, int pageSize, int pageNumber)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/students/{phoneNumberSuffix}/phone-number?pageSize={pageSize}&pageNumber={pageNumber}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync(client.BaseAddress);

            var result = await response.Content.ReadAsStringAsync();

            List<StudentForResultDto> students = JsonConvert.DeserializeObject<List<StudentForResultDto>>(result)!;

            var pageNavigation = response.Headers.GetValues("X-Pagination").FirstOrDefault();
            var pagination = JsonConvert.DeserializeObject<PaginationMetadata>(pageNavigation!);

            return new PagedResponse<StudentForResultDto>
            {
                Data = students,
                PageSize = pagination.PageSize,
                CurrentPage = pagination.CurrentPage,
                HasNext = pagination.HasNext,
                HasPrevious = pagination.HasPrevious
            };
        }
        catch(Exception ex)
        {
            return new PagedResponse<StudentForResultDto>();
        }
    }

    public async Task<bool> UpdateAsync(long id, StudentForUpdateDto dto)
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

    public async Task<bool> UpdateStudentByGroupAsync(long id, long groupId, Status status)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/students/{id}/group/{groupId}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = new HttpRequestMessage(HttpMethod.Put, client.BaseAddress)
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(status),
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
}

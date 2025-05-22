using EduFlow.BLL.DTOs.Users.Teacher;
using EduFlow.Desktop.Integrated.Api.Auth;
using EduFlow.Desktop.Integrated.Security;
using EduFlow.Desktop.Integrated.Servers.Interfaces.Users.Teacher;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace EduFlow.Desktop.Integrated.Servers.Repositories.Users.Teacher;

public class TeacherServer : ITeacherServer
{
    public async Task<bool> AddAsync(TeacherForCreateDto dto)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/teachers");
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

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/teachers/{id}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.DeleteAsync(client.BaseAddress);
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

    public async Task<List<TeacherForResultDto>> GetAllAsync()
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/teachers");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage message = await client.GetAsync(client.BaseAddress);
            var response = await message.Content.ReadAsStringAsync();

            List<TeacherForResultDto> result = JsonConvert.DeserializeObject<List<TeacherForResultDto>>(response)!;

            return result;
        }
        catch(Exception ex)
        {
            return new List<TeacherForResultDto>();
        }
    }

    public async Task<List<TeacherForResultDto>> GetAllByCourseIdAsync(long courseId)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/teachers/{courseId}/course");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage message = await client.GetAsync(client.BaseAddress);
            var response = await message.Content.ReadAsStringAsync();

            List<TeacherForResultDto> result = JsonConvert.DeserializeObject<List<TeacherForResultDto>>(response)!;

            return result;
        }
        catch(Exception ex)
        {
            return new List<TeacherForResultDto>();
        }
    }

    public async Task<TeacherForResultDto> GetByIdAsync(long id)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/teachers/{id}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage message = await client.GetAsync(client.BaseAddress);

            var response = await message.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<TeacherForResultDto>(response)!;

            return result;
        }
        catch(Exception ex)
        {
            return new TeacherForResultDto();
        }
    }

    public async Task<TeacherForResultDto> GetByUserIdAsync(long userId)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/teachers/{userId}/user");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage message = await client.GetAsync(client.BaseAddress);

            var response = await message.Content.ReadAsStringAsync();
            
            var result = JsonConvert.DeserializeObject<TeacherForResultDto>(response)!;
            
            return result;
        }
        catch (Exception ex)
        {
            return new TeacherForResultDto();
        }
    }

    public async Task<bool> UpdateAsync(long id, TeacherForUpdateDto dto)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/teachers/{id}");
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

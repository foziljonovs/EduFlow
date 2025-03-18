using EduFlow.BLL.DTOs.Users.User;
using EduFlow.Desktop.Integrated.Api.Auth;
using EduFlow.Desktop.Integrated.Security;
using EduFlow.Desktop.Integrated.Servers.Interfaces.Users.User;
using Newtonsoft.Json;
using System.Text;

namespace EduFlow.Desktop.Integrated.Servers.Repositories.Users.User;

public class UserServer : IUserServer
{
    public async Task<bool> ChangePasswordAsync(long id, UserForChangePasswordDto dto)
    {
        try
        {
            HttpClient client = new HttpClient();
            var url = $"{AuthApi.BASE_URL}/api/users/{id}/change-password";
            var request = new HttpRequestMessage(HttpMethod.Post, url)
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

    public async Task<bool> DeleteAsync(long id)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/users/{id}");
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = await client.DeleteAsync(client.BaseAddress);

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

    public async Task<List<UserForResultDto>> GetAllAsync()
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/users");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue($"Bearer {token}");

            HttpResponseMessage message = await client.GetAsync(client.BaseAddress);

            var response = await message.Content.ReadAsStringAsync();

            List<UserForResultDto> users = JsonConvert.DeserializeObject<List<UserForResultDto>>(response)!;
            return users;
        }
        catch(Exception ex)
        {
            return new List<UserForResultDto>();
        }
    }

    public async Task<UserForResultDto> GetByIdAsync(long id)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/users/{id}");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue($"Bearer {token}");

            HttpResponseMessage message = await client.GetAsync(client.BaseAddress);

            var response = await message.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<UserForResultDto>(response)!;
            return user;
        }
        catch(Exception ex)
        {
            return new UserForResultDto();
        }
    }

    public async Task<bool> RegisterAsync(UserForCreateDto dto)
    {
        try
        {
            using(var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(30);
                using(var request = new HttpRequestMessage(HttpMethod.Post, AuthApi.BASE_URL + "/api/users"))
                {
                    var json = JsonConvert.SerializeObject(dto);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    request.Content = content;

                    var response = await client.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                        return true;
                    else
                        return false;
                }
            }
        }
        catch(Exception ex)
        {
            return false;
        }
    }

    public async Task<bool> UpdateAsync(long id, UserForUpdateDto dto)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            var url = $"{AuthApi.BASE_URL}/api/users/{id}";
            var request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Headers.Add("Authorization", $"Bearer {token}");

            request.Content = new StringContent(
                JsonConvert.SerializeObject(dto),
                Encoding.UTF8,
                "application/json");

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

    public async Task<bool> VerifyPasswordAsync(long id, string password)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            var url = $"{AuthApi.BASE_URL}/api/users/{id}/verify?password={password}";
            var request = new HttpRequestMessage(HttpMethod.Get, url);

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

using EduFlow.BLL.DTOs.Companies.Logo;
using EduFlow.Desktop.Integrated.Api.Auth;
using EduFlow.Desktop.Integrated.Servers.Interfaces.Companies.Logo;
using Newtonsoft.Json;
using System.Text;

namespace EduFlow.Desktop.Integrated.Servers.Repositories.Companies.Logo;

public class LogoServer : ILogoServer
{
    public async Task<long> AddAsync(LogoForCreateDto dto)
    {
        try
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/logos");

            var request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress)
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(dto),
                    Encoding.UTF8,
                    "application/json")
            };

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<long>(content);
                return result;
            }
            else
                return 0;
        }
        catch (Exception ex)
        {
            return 0;
        }
    }

    public async Task<bool> DeleteAsync(long id)
    {
        try
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/logos/{id}");

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

    public async Task<List<LogoForResultDto>> GetAllAsync()
    {
        try
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/logos");

            var response = await client.GetAsync(client.BaseAddress);

            var result = await response.Content.ReadAsStringAsync();

            List<LogoForResultDto> logos = JsonConvert.DeserializeObject<List<LogoForResultDto>>(result)!;

            return logos;
        }
        catch(Exception ex)
        {
            return new List<LogoForResultDto>();
        }
    }

    public async Task<LogoForResultDto> GetByIdAsync(long id)
    {
        try
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/logos/{id}");

            var response = await client.GetAsync(client.BaseAddress);

            var result = await response.Content.ReadAsStringAsync();

            LogoForResultDto logo = JsonConvert.DeserializeObject<LogoForResultDto>(result)!;

            return logo;
        }
        catch (Exception ex)
        {
            return new LogoForResultDto();
        }
    }

    public async Task<bool> UpdateAsync(long id, LogoForUpdateDto dto)
    {
        try
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/logos/{id}");

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

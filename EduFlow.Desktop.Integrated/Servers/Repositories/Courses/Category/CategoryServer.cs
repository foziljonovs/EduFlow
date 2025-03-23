using EduFlow.BLL.DTOs.Courses.Category;
using EduFlow.Desktop.Integrated.Api.Auth;
using EduFlow.Desktop.Integrated.Security;
using EduFlow.Desktop.Integrated.Servers.Interfaces.Courses.Category;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace EduFlow.Desktop.Integrated.Servers.Repositories.Courses.Category;

public class CategoryServer : ICategoryServer
{
    public async Task<bool> AddAsync(CategoryForCraeteDto dto)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/categories");
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

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/categories/{id}");
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

    public async Task<List<CategoryForResultDto>> GetAllAsync()
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/categories");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync(client.BaseAddress);

            var content = await response.Content.ReadAsStringAsync();
            
            var result = JsonConvert.DeserializeObject<List<CategoryForResultDto>>(content)!;
            
            return result;
        }
        catch (Exception ex)
        {
            return new List<CategoryForResultDto>();
        }
    }

    public async Task<CategoryForResultDto> GetByIdAsync(long id)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;
            
            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/categories/{id}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var response = await client.GetAsync(client.BaseAddress);
            
            var content = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<CategoryForResultDto>(content)!;

            return result;
        }
        catch (Exception ex)
        {
            return new CategoryForResultDto();
        }
    }

    public async Task<bool> UpdateAsync(long id, CategoryForUpdateDto dto)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;
            
            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/categories/{id}");
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

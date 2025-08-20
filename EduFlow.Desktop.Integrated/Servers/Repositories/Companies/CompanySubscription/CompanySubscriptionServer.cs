using EduFlow.BLL.DTOs.Companies.CompanySubscription;
using EduFlow.Desktop.Integrated.Api.Auth;
using EduFlow.Desktop.Integrated.Security;
using EduFlow.Desktop.Integrated.Servers.Interfaces.Companies.CompanySubscription;
using Newtonsoft.Json;
using System.Text;

namespace EduFlow.Desktop.Integrated.Servers.Repositories.Companies.CompanySubscription;

public class CompanySubscriptionServer : ICompanySubscriptionServer
{
    public async Task<bool> AddAsync(CompanySubscriptionForCreateDto dto)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/company/subscriptions");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

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

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/company/subscriptions/{id}");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await client.DeleteAsync(client.BaseAddress);
            
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

    public async Task<List<CompanySubscriptionForResultDto>> GetAllAsync()
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/company/subscriptions");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync(client.BaseAddress);

            var result = await response.Content.ReadAsStringAsync();

            List<CompanySubscriptionForResultDto> subscriptions = JsonConvert.DeserializeObject<List<CompanySubscriptionForResultDto>>(result)!;

            return subscriptions;
        }
        catch (Exception ex)
        {
            return new List<CompanySubscriptionForResultDto>();
        }
    }

    public async Task<List<CompanySubscriptionForResultDto>> GetAllByCompanyIdAsync(long companyId)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/company/subscriptions/company/{companyId}");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync(client.BaseAddress);

            var result = await response.Content.ReadAsStringAsync();

            List<CompanySubscriptionForResultDto> subscriptions = JsonConvert.DeserializeObject<List<CompanySubscriptionForResultDto>>(result)!;

            return subscriptions;
        }
        catch (Exception ex)
        {
            return new List<CompanySubscriptionForResultDto>();
        }
    }

    public async Task<List<CompanySubscriptionForResultDto>> GetAllIsActive()
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/company/subscriptions/active");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync(client.BaseAddress);

            var result = await response.Content.ReadAsStringAsync();

            List<CompanySubscriptionForResultDto> subscriptions = JsonConvert.DeserializeObject<List<CompanySubscriptionForResultDto>>(result)!;

            return subscriptions;
        }
        catch (Exception ex)
        {
            return new List<CompanySubscriptionForResultDto>();
        }
    }

    public async Task<CompanySubscriptionForResultDto> GetByIdAsync(long id)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/company/subscriptions/{id}");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync(client.BaseAddress);

            var result = await response.Content.ReadAsStringAsync();

            CompanySubscriptionForResultDto subscription = JsonConvert.DeserializeObject<CompanySubscriptionForResultDto>(result)!;

            return subscription;
        }
        catch (Exception ex)
        {
            return new CompanySubscriptionForResultDto();
        }
    }

    public async Task<bool> UpdateAsync(long id, CompanySubscriptionForUpdateDto dto)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/company/subscriptions/{id}");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

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

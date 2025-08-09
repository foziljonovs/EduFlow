using EduFlow.BLL.DTOs.Companies.Company;
using EduFlow.Desktop.Integrated.Api.Auth;
using EduFlow.Desktop.Integrated.Security;
using EduFlow.Desktop.Integrated.Servers.Interfaces.Companies.Company;
using Newtonsoft.Json;
using System.Text;

namespace EduFlow.Desktop.Integrated.Servers.Repositories.Companies.Company;

public class CompanyServer : ICompanyServer
{
    public async Task<bool> AddAsync(CompanyForCreateDto dto)
    {
        try
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/companies");

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

    public async Task<bool> DeleteAsync(long id)
    {
        try
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/companies/{id}");

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

    public async Task<List<CompanyForResultDto>> GetAllAsync()
    {
        try
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/companies");

            var response = await client.GetAsync(client.BaseAddress);

            var result = await response.Content.ReadAsStringAsync();

            List<CompanyForResultDto> companies = JsonConvert.DeserializeObject<List<CompanyForResultDto>>(result)!;

            return companies;
        }
        catch(Exception ex)
        {
            return new List<CompanyForResultDto>();
        }
    }

    public async Task<CompanyForResultDto> GetByIdAsync(long id)
    {
        try
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/companies/{id}");

            var response = await client.GetAsync(client.BaseAddress); 

            var result = await response.Content.ReadAsStringAsync();

            CompanyForResultDto company = JsonConvert.DeserializeObject<CompanyForResultDto>(result)!;

            return company;
        }
        catch(Exception ex)
        {
            return new CompanyForResultDto();
        }
    }

    public async Task<bool> UpdateAsync(long id, CompanyForUpdateDto dto)
    {
        try
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/companies/{id}");

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

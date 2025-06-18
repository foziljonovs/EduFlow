using EduFlow.BLL.DTOs.Payments.Payment;
using EduFlow.Desktop.Integrated.Api.Auth;
using EduFlow.Desktop.Integrated.Security;
using EduFlow.Desktop.Integrated.Servers.Interfaces.Payments;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace EduFlow.Desktop.Integrated.Servers.Repositories.Payments;

public class PaymentServer : IPaymentServer
{
    public async Task<bool> AddToPayAsync(PaymentForCreateDto dto)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/payments");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress)
            {
                Content = new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(dto),
                    System.Text.Encoding.UTF8,
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

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/payments/{id}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = new HttpRequestMessage(HttpMethod.Delete, client.BaseAddress);

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

    public async Task<List<PaymentForResultDto>> GetAllAsync()
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/payments");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync(client.BaseAddress);

            var result = await response.Content.ReadAsStringAsync();

            List<PaymentForResultDto> payments = JsonConvert.DeserializeObject<List<PaymentForResultDto>>(result)!;

            return payments;
        }
        catch(Exception ex)
        {
            return new List<PaymentForResultDto>();
        }
    }

    public async Task<List<PaymentForResultDto>> GetAllByGroupIdAsync(long groupId)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/payments/{groupId}/group");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync(client.BaseAddress);

            var result = await response.Content.ReadAsStringAsync();

            List<PaymentForResultDto> payments = JsonConvert.DeserializeObject<List<PaymentForResultDto>>(result)!;

            return payments;
        }
        catch(Exception ex)
        {
            return new List<PaymentForResultDto>();
        }
    }

    public async Task<List<PaymentForResultDto>> GetAllByStudentIdAsync(long studentId)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/payments/{studentId}/student");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync(client.BaseAddress);
            
            var result = await response.Content.ReadAsStringAsync();
            
            List<PaymentForResultDto> payments = JsonConvert.DeserializeObject<List<PaymentForResultDto>>(result)!;
            
            return payments;
        }
        catch(Exception ex)
        {
            return new List<PaymentForResultDto>();
        }
    }

    public async Task<PaymentForResultDto> GetByIdAsync(long id)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/payments/{id}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var response = await client.GetAsync(client.BaseAddress);
            
            var result = await response.Content.ReadAsStringAsync();
            
            PaymentForResultDto payment = System.Text.Json.JsonSerializer.Deserialize<PaymentForResultDto>(result)!;
            
            return payment;
        }
        catch(Exception ex)
        {
            return new PaymentForResultDto();
        }
    }

    public async Task<bool> UpdateToPayAsync(long id, PaymentForUpdateDto dto)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;
            
            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/payments/{id}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var request = new HttpRequestMessage(HttpMethod.Put, client.BaseAddress)
            {
                Content = new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(dto),
                    System.Text.Encoding.UTF8,
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

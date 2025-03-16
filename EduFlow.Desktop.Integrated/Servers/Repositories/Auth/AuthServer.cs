using EduFlow.BLL.DTOs.Users.User;
using EduFlow.Desktop.Integrated.Api.Auth;
using EduFlow.Desktop.Integrated.Servers.Interfaces.Auth;
using Newtonsoft.Json;
using System.Text;

namespace EduFlow.Desktop.Integrated.Servers.Repositories.Auth;

public class AuthServer : IAuthServer
{
    public async Task<(bool result, string token)> LoginAsync(UserForLoginDto dto)
    {
        try
        {
            HttpClient client = new HttpClient();
            using var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{AuthApi.LOGIN_URL}")
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(dto),
                    Encoding.UTF8,
                    "application/json")
            };

            var response = await client.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                string token = await response.Content.ReadAsStringAsync();
                return (result: true, token: token);
            }
            
            return (result: false, token: string.Empty);
        }
        catch(Exception ex)
        {
            return (result: false, token: string.Empty);
        }
    }
}

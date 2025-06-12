using EduFlow.SharedConfig;

namespace EduFlow.Desktop.Integrated.Api.Auth;

public static class AuthApi
{
    public static string BASE_URL => ConfigurationManager.GetValue("ApiConfig:BaseUrl");
    public static readonly string LOGIN_URL = BASE_URL + "/api/auth/login";
}

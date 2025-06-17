using EduFlow.SharedConfig;

namespace EduFlow.Desktop.Integrated.Api.Auth;

public static class AuthApi
{
    public static string BASE_URL => ConfigurationManager.GetValue("ApiConfig:BaseUrl");
    //public static readonly string BASE_URL = "http://185.191.141.237";
    public static readonly string LOGIN_URL = BASE_URL + "/api/auth/login";
}

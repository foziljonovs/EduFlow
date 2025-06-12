using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace EduFlow.SharedConfig;

public static class ConfigurationManager
{
    private static IConfiguration _configuration;

    static ConfigurationManager()
    {
        var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";

        _configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
    }

    public static string GetValue(string key)
        => _configuration[key];

    public static IConfigurationSection GetSection(string key)
        => _configuration.GetSection(key);
}

using EduFlow.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace EduFlow.WebApi.Configurations;

public static class LayerConfiguration
{
    public static IServiceCollection AddDbConnection(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("localhost");

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        return services;
    }
}

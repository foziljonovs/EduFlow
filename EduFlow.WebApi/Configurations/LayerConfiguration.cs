using EduFlow.DAL.Data;
using EduFlow.DAL.Interfaces;
using EduFlow.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

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

    public static IServiceCollection AddRepositoryConfiguration(
        this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    public static void AddSerilogConfiguration(IHostBuilder host)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(
                path: "logs/edu-flow-log.txt",
                rollingInterval: RollingInterval.Month)
            .CreateLogger();

        host.UseSerilog();
    }
}

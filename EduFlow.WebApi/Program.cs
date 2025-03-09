using EduFlow.BLL.Hubs;
using EduFlow.WebApi.Configurations;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

LayerConfiguration.AddSerilogConfiguration(builder.Host);

builder.Services.AddControllers();

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddOpenApi()
    .AddDbConnection(builder.Configuration)
    .AddRepositoryConfiguration()
    .AddCorsConfiguration()
    .AddSwaggerConfigurtion()
    .AddJwtConfiguration(builder.Configuration)
    .AddSignalR();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<NotificationHub>("/notification-hub");

app.Run();

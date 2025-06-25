using EduFlow.BLL.Common.Mapping;
using EduFlow.BLL.Hubs;
using EduFlow.WebApi.Configurations;
using EduFlow.WebApi.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

LayerConfiguration.AddSerilogConfiguration(builder.Host);

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddOpenApi()
    .AddDbConnection(builder.Configuration)
    .AddRepositoryConfiguration()
    .AddServiceConfiguration()
    .AddValidationServiceConfiguration()
    .AddValidationConfiguration()
    .AddCorsConfiguration()
    .AddSwaggerConfigurtion()
    .AddJwtConfiguration(builder.Configuration)
    .AddAutoMapper(typeof(MappingProfile))
    .AddSignalR();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.Secure = CookieSecurePolicy.None;
});

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSerilogRequestLogging();

app.UseRouting();

app.UseCors("AllowAll");

//app.UseMiddleware<ExceptionMiddleware>();

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<NotificationHub>("/notification-hub")
    .RequireCors("AllowAll");

app.Run();

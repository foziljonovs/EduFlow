using EduFlow.BLL.Common.Exceptions;
using FluentValidation;
using Newtonsoft.Json;
using System.Net;

namespace EduFlow.WebApi.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(
        RequestDelegate next, 
        ILogger<ExceptionMiddleware> logger)
    {
        this._next = next;
        this._logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            int statusCode = ex switch
            {
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                StatusCodeException => (int)HttpStatusCode.NotFound,
                ValidationException => (int)HttpStatusCode.BadRequest
            };

            string errorMessage = ex switch
            {
                UnauthorizedAccessException => "Unauthorized access.",
                StatusCodeException statusCodeException => ex.Message,
                ValidationException validationException => ex.Message,
                _ => "An unexpected error occurred."
            };

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(JsonConvert.SerializeObject(new
            {
                StatusCode = statusCode,
                Message = errorMessage
            }));
        }
    }
}

using Common.Models.Response;
using System.Net;
using System.Text.Json;

namespace AppAPI.Infrastructure.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(
            new EventId(exception.HResult),
            exception,
            $"{exception?.Message}\n{exception?.InnerException?.Message}");

        var errorMessage = "An error occurred. Please try again later.";
        var code = (int)HttpStatusCode.InternalServerError;
        if (exception is Common.Exceptions.HttpRequestException e)
        {
            code = (int)e.StatusCode;
            errorMessage = e.Message;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = code;
        errorMessage = JsonSerializer.Serialize(new ErrorResult(errorMessage, code), new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        return context.Response.WriteAsync(errorMessage);
    }
}

public static class ExceptionHandlingMiddlewareExtension
{
    public static void UseExceptionHandling(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}

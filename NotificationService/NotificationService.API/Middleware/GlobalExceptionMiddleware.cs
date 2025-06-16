using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using JobTracker.NotificationService.Application.CustomExceptions;

namespace JobTracker.NotificationService.Api.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, title, detail) = GetProblemDetails(exception);

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = _env.IsDevelopment() ? detail : null,
            Instance = context.Request.Path
        };

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        await context.Response.WriteAsync(JsonSerializer.Serialize(problem, options));
    }

    private (int statusCode, string title, string detail) GetProblemDetails(Exception ex)
    {
        return ex switch
        {
            ArgumentException => ((int)HttpStatusCode.BadRequest, "Invalid argument", ex.Message),
            UnauthorizedAccessException => ((int)HttpStatusCode.Unauthorized, "Unauthorized access", ex.Message),
            InvalidOperationException => ((int)HttpStatusCode.BadRequest, "Invalid operation", ex.Message),
            NotFoundException => ((int)HttpStatusCode.NotFound, "Resource not found", ex.Message),
            _ => ((int)HttpStatusCode.InternalServerError, "Internal Server Error", ex.Message)
        };
    }
}
using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace JobTracker.SharedKernel.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly ExceptionHandlerContext _handlerContext;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _handlerContext = new ExceptionHandlerContext(env);
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred.");
                var problem = _handlerContext.Handle(ex, context);

                problem.Type = $"{context.Request.Scheme}://{context.Request.Host}/api/errors/{ex.GetType().Name}";
                problem.Extensions["traceId"] = Activity.Current?.Id ?? context.TraceIdentifier;
                problem.Extensions["requestId"] = context.TraceIdentifier;

                context.Response.ContentType = "application/problem+json";
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                await context.Response.WriteAsync(JsonSerializer.Serialize(problem, options));
            }
        }
    }
}
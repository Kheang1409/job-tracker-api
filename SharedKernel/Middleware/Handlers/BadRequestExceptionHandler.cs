using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobTracker.SharedKernel.Middleware.Handlers;

public sealed class BadRequestExceptionHandler : ExceptionHandlerBase
{
    public override ProblemDetails? Handle(Exception exception, HttpContext context, bool isDevelopment)
    {
        if (exception is not (ArgumentException or InvalidOperationException))
            return base.Handle(exception, context, isDevelopment);

        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        return new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Invalid request",
            Detail = exception.Message,
            Instance = $"{context.Request.Method} {context.Request.Path}"
        };
    }
}

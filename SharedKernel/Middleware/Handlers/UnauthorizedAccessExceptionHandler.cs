using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobTracker.SharedKernel.Middleware.Handlers
{
    public class UnauthorizedAccessExceptionHandler : ExceptionHandlerBase
    {
        public override ProblemDetails? Handle(Exception exception, HttpContext context, bool isDevelopment)
        {
            if (exception is UnauthorizedAccessException unauthorizedEx)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                return new ProblemDetails
                {
                    Status = StatusCodes.Status401Unauthorized,
                    Title = "Unauthorized Access",
                    Detail = isDevelopment ? unauthorizedEx.Message : null,
                    Instance = $"{context.Request.Method} {context.Request.Path}"
                };
            }

            return base.Handle(exception, context, isDevelopment);
        }
    }
}

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
                var isLoginRequest = context.Request.Path.Equals("/api/auth/login", StringComparison.OrdinalIgnoreCase);

                var problem = new ProblemDetails
                {
                    Status = StatusCodes.Status401Unauthorized,
                    Title = "Unauthorized Access",
                    Detail = isDevelopment || isLoginRequest ? unauthorizedEx.Message : null,
                    Instance = $"{context.Request.Method} {context.Request.Path}"
                };

                if (isLoginRequest)
                {
                    problem.Extensions["code"] = unauthorizedEx.Message.Contains(
                        "verify your email",
                        StringComparison.OrdinalIgnoreCase)
                        ? "email-not-verified"
                        : "invalid-credentials";
                }

                return problem;
            }

            return base.Handle(exception, context, isDevelopment);
        }
    }
}

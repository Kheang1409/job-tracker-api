using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobTracker.SharedKernel.Middleware.Handlers
{
    public class ValidationExceptionHandler : ExceptionHandlerBase
    {
        public override ProblemDetails? Handle(Exception exception, HttpContext context, bool isDevelopment)
        {
            if (exception is ValidationException validationEx)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                var errors = validationEx.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );

                var problem = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Validation Failed",
                    Detail = isDevelopment ? validationEx.Message : null,
                    Instance = $"{context.Request.Method} {context.Request.Path}"
                };

                problem.Extensions["errors"] = errors;
                return problem;
            }

            return base.Handle(exception, context, isDevelopment);
        }
    }
}
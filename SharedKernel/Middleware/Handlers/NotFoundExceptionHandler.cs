using JobTracker.SharedKernel.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobTracker.SharedKernel.Middleware.Handlers
{
    public class NotFoundExceptionHandler : ExceptionHandlerBase
    {
        public override ProblemDetails? Handle(Exception exception, HttpContext context, bool isDevelopment)
        {
            if (exception is NotFoundException notFoundEx)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;

                return new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Resource Not Found",
                    Detail = isDevelopment ? notFoundEx.Message : null,
                    Instance = $"{context.Request.Method} {context.Request.Path}"
                };
            }

            return base.Handle(exception, context, isDevelopment);
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobTracker.SharedKernel.Middleware.Handlers
{
    public interface IExceptionHandler
    {
        IExceptionHandler SetNext(IExceptionHandler handler);
        ProblemDetails? Handle(Exception exception, HttpContext context, bool isDevelopment);
    }
}

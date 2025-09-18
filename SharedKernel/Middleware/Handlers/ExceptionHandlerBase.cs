using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobTracker.SharedKernel.Middleware.Handlers
{
    public abstract class ExceptionHandlerBase : IExceptionHandler
    {
        private IExceptionHandler _next;

        public IExceptionHandler SetNext(IExceptionHandler handler)
        {
            _next = handler;
            return handler;
        }

        public virtual ProblemDetails? Handle(Exception exception, HttpContext context, bool isDevelopment)
        {
            return _next?.Handle(exception, context, isDevelopment);
        }
    }
}

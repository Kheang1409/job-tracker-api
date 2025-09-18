using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using JobTracker.SharedKernel.Middleware.Handlers;

namespace JobTracker.SharedKernel.Middleware
{
    public class ExceptionHandlerContext
    {
        private readonly IExceptionHandler _chain;

        public ExceptionHandlerContext(IHostEnvironment env)
        {
            var validation = new ValidationExceptionHandler();
            var unauthorized = new UnauthorizedAccessExceptionHandler();
            var notFound = new NotFoundExceptionHandler();
            var fallback = new DefaultExceptionHandler();

            validation
                .SetNext(unauthorized)
                .SetNext(notFound)
                .SetNext(fallback);

            _chain = validation;
            IsDevelopment = env.IsDevelopment();
        }

        private bool IsDevelopment { get; }

        public ProblemDetails Handle(Exception ex, HttpContext context)
        {
            return _chain.Handle(ex, context, IsDevelopment)!;
        }
    }
}
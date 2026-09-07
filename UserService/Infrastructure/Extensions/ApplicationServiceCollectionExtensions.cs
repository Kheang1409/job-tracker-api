using FluentValidation;
using JobTracker.SharedKernel.Behaviors;
using JobTracker.UserService.Application.Auths.Commands.ForgotPassword;
using JobTracker.UserService.Application.Services;
using JobTracker.UserService.Infrastructure.Services;
using MediatR;

namespace JobTracker.UserService.Infrastructure.Extensions;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblyContaining<ForgotPasswordCommand>());
        services.AddValidatorsFromAssemblyContaining<ForgotPasswordCommand>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddScoped<IJwtService, JwtService>();

        return services;
    }
}

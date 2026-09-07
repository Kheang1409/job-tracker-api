using FluentValidation;
using JobTracker.JobApplicationService.Application.JobApplications;
using JobTracker.JobApplicationService.Application.Repositories;
using JobTracker.JobApplicationService.Infrastructure.Repositories;
using JobTracker.SharedKernel.Behaviors;
using JobTracker.SharedKernel.Extensions;
using MediatR;

namespace JobTracker.JobApplicationService.Infrastructure.Persistence;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddJobApplicationInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        JobApplicationMongoMappings.Register();
        services.AddJwtAuthentication(configuration);
        services.AddSwaggerDocumentation("Job Application API");
        services.AddMediatR(options => options.RegisterServicesFromAssemblyContaining<GetJobApplicationsQuery>());
        services.AddValidatorsFromAssemblyContaining<GetJobApplicationsQuery>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddMongoDb(configuration, context =>
            context.AddSingleton<IJobApplicationRepository, JobApplicationRepository>());
        services.AddHostedService<JobApplicationIndexInitializer>();
        return services;
    }
}

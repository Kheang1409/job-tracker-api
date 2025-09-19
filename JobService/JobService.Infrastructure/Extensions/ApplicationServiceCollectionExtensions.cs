using JobTracker.JobService.Application.JobLocations.Commands.CreatePost;
using Microsoft.Extensions.DependencyInjection;
using JobTracker.SharedKernel.Behaviors;
using FluentValidation;
using MediatR;
using JobTracker.JobService.Application.JobLocations.Queries.GetPost;
using JobTracker.JobService.Application.JobLocations.Queries.GetPosts;
using JobTracker.JobService.Application.JobLocations.Commands.UpdatePost;
using JobTracker.JobService.Application.JobLocations.Commands.DeletePost;
using JobTracker.JobService.Application.JobLocations.Commands.UpdatePostStatus;
using JobTracker.JobService.Application.JobLocations.Queries.GetPostCount;

namespace JobTracker.JobService.Infrastructure.Extensions;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            // Posts
            cfg.RegisterServicesFromAssemblyContaining<CreatePostCommand>();
            cfg.RegisterServicesFromAssemblyContaining<CreatePostWithIdCommand>();
            cfg.RegisterServicesFromAssemblyContaining<UpdatePostCommand>();
            cfg.RegisterServicesFromAssemblyContaining<UpdatePostWithIdCommand>();
            cfg.RegisterServicesFromAssemblyContaining<UpdateStatusPostCommand>();
            cfg.RegisterServicesFromAssemblyContaining<UpdatePostStatusWithIdCommand>();
            cfg.RegisterServicesFromAssemblyContaining<DeletePostCommand>();
            cfg.RegisterServicesFromAssemblyContaining<GetPostQuery>();
            cfg.RegisterServicesFromAssemblyContaining<GetPostsQuery>();
            cfg.RegisterServicesFromAssemblyContaining<GetPostCountQuery>();

        });

        // Posts
        services.AddValidatorsFromAssemblyContaining<CreatePostCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdatePostCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdatePostStatusCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdatePostCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<GetPostQueryValidator>();
        services.AddValidatorsFromAssemblyContaining<GetPostsQueryValidator>();
        services.AddValidatorsFromAssemblyContaining<GetPostCountQueryValidator>();
        

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}

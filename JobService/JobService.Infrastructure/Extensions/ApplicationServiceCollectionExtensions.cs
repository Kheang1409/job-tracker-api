using JobTracker.JobService.Application.JobLocations.Commands.CreateJobPost;
using JobTracker.JobService.Application.Behaviors;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using MediatR;
using JobTracker.JobService.Application.JobLocations.Queries.GetJobPost;
using JobTracker.JobService.Application.JobLocations.Queries.GetJobPosts;
using JobTracker.JobService.Application.JobLocations.Commands.UpdateJobPost;
using JobTracker.JobService.Application.JobLocations.Commands.DeleteJobPost;
using JobTracker.JobService.Application.JobLocations.Commands.UpdateStatusJobPost;
using JobTracker.JobService.Application.Skills.Commands.CreateSkill;
using JobTracker.JobService.Application.Skills.Commands.DeleteSkill;
using JobTracker.JobService.Application.Skills.Queries.GetSkill;
using JobTracker.JobService.Application.Skills.Queries.GetSkills;

namespace JobTracker.JobService.Infrastructure.Extensions;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            //Job Posts
            cfg.RegisterServicesFromAssemblyContaining<CreateJobPostCommand>();
            cfg.RegisterServicesFromAssemblyContaining<CreateJobPostWithIdCommand>();
            cfg.RegisterServicesFromAssemblyContaining<UpdateJobPostCommand>();
            cfg.RegisterServicesFromAssemblyContaining<UpdateJobPostWithIdCommand>();
            cfg.RegisterServicesFromAssemblyContaining<UpdateStatusJobPostCommand>();
            cfg.RegisterServicesFromAssemblyContaining<UpdateStatusJobPostWithIdCommand>();
            cfg.RegisterServicesFromAssemblyContaining<DeleteJobPostCommand>();
            cfg.RegisterServicesFromAssemblyContaining<GetJobPostQuery>();
            cfg.RegisterServicesFromAssemblyContaining<GetJobPostsQuery>();

            //Skills
            cfg.RegisterServicesFromAssemblyContaining<CreateSkillCommand>();
            cfg.RegisterServicesFromAssemblyContaining<CreateSkillWithIdCommand>();
            cfg.RegisterServicesFromAssemblyContaining<DeleteSkillCommand>();
            cfg.RegisterServicesFromAssemblyContaining<GetSkillQuery>();
            cfg.RegisterServicesFromAssemblyContaining<GetSkillsQuery>();



        });

        //Job Posts
        services.AddValidatorsFromAssemblyContaining<CreateJobPostCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateJobPostCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateStatusJobPostCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateJobPostCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<GetJobPostQueryValidator>();
        services.AddValidatorsFromAssemblyContaining<GetJobPostsQueryValidator>();

        //Skills
        services.AddValidatorsFromAssemblyContaining<CreateSkillCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<DeleteSkillCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<GetSkillQueryValidator>();
        services.AddValidatorsFromAssemblyContaining<GetSkillsQueryValidator>();
        

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}

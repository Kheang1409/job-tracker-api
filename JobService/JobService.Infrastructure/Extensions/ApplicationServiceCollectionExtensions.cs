using JobTracker.JobService.Application.JobLocations.Commands.CreateJobPost;
using Microsoft.Extensions.DependencyInjection;
using JobTracker.SharedKernel.Behaviors;
using FluentValidation;
using MediatR;
using JobTracker.JobService.Application.JobLocations.Queries.GetJobPost;
using JobTracker.JobService.Application.JobLocations.Queries.GetJobPosts;
using JobTracker.JobService.Application.JobLocations.Commands.UpdateJobPost;
using JobTracker.JobService.Application.JobLocations.Commands.DeleteJobPost;
using JobTracker.JobService.Application.JobLocations.Commands.UpdateStatusJobPost;
using JobTracker.JobService.Application.Candidates.Applied.Commands;
using JobTracker.JobService.Application.Candidates.Queries.GetCandidate;
using JobTracker.JobService.Application.Candidates.Queries.GetCandidates;
using JobTracker.JobService.Application.Candidates.Withdraw.Commands;
using JobTracker.JobService.Application.Candidates.MoveOn.Commands;
using JobTracker.JobService.Application.Candidates.Rejected.Commands;
using JobTracker.JobService.Application.Candidates.Selected.Commands;
using JobTracker.JobService.Application.JobLocations.Queries.GetJobCount;

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
            cfg.RegisterServicesFromAssemblyContaining<GetJobCountQuery>();
            

            //Candidates
            cfg.RegisterServicesFromAssemblyContaining<AppliedCommand>();
            cfg.RegisterServicesFromAssemblyContaining<WithdrawCommand>();
            cfg.RegisterServicesFromAssemblyContaining<MoveOnCommand>();
            cfg.RegisterServicesFromAssemblyContaining<RejectedCommand>();
            cfg.RegisterServicesFromAssemblyContaining<SelectedCommand>();
            

            cfg.RegisterServicesFromAssemblyContaining<GetCandidateQuery>();
            cfg.RegisterServicesFromAssemblyContaining<GetCandidatesQuery>();

        });

        //Job Posts
        services.AddValidatorsFromAssemblyContaining<CreateJobPostCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateJobPostCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateStatusJobPostCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateJobPostCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<GetJobPostQueryValidator>();
        services.AddValidatorsFromAssemblyContaining<GetJobPostsQueryValidator>();
        services.AddValidatorsFromAssemblyContaining<GetJobCountQueryValidator>();

        //Candidate
        services.AddValidatorsFromAssemblyContaining<AppliedCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<WithdrawCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<MoveOnCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<RejectedCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<SelectedCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<GetCandidateQueryValidator>();
        services.AddValidatorsFromAssemblyContaining<GetCandidatesQueryValidator>();
        

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}

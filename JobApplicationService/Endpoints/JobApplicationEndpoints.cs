using JobTracker.JobApplicationService.Contracts.JobApplications;
using JobTracker.JobApplicationService.Hubs;
using JobTracker.JobApplicationService.Application.JobApplications;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace JobTracker.JobApplicationService.Endpoints;

public static class JobApplicationEndpoints
{
    public static IEndpointRouteBuilder MapJobApplicationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var applications = endpoints.MapGroup("/api/job-applications").RequireAuthorization();
        applications.MapGet("", GetApplications);
        applications.MapPost("", CreateApplication);
        applications.MapPut("/{id}", UpdateApplication);
        applications.MapDelete("/{id}", DeleteApplication);
        return endpoints;
    }

    private static async Task<IResult> GetApplications(HttpContext context, IMediator mediator) =>
        Results.Ok(await mediator.Send(new GetJobApplicationsQuery(context.GetUserId())));

    private static async Task<IResult> CreateApplication(CreateJobApplicationRequest request, HttpContext context, IMediator mediator, IHubContext<JobApplicationsHub> hub)
    {
        var userId = context.GetUserId();
        var application = await mediator.Send(new CreateJobApplicationCommand(
            userId, request.Company, request.Role, request.Source, request.Status,
            request.AppliedOn, request.Url, request.Notes, request.ReminderAt));
        await hub.Clients.User(userId).SendAsync("ApplicationCreated", application);
        return Results.Created($"/api/job-applications/{application.Id}", application);
    }

    private static async Task<IResult> UpdateApplication(string id, UpdateJobApplicationRequest request, HttpContext context, IMediator mediator, IHubContext<JobApplicationsHub> hub)
    {
        var userId = context.GetUserId();
        var application = await mediator.Send(new UpdateJobApplicationCommand(
            userId, id, request.Company, request.Role, request.Source, request.Status,
            request.AppliedOn, request.Url, request.Notes, request.ReminderAt));
        await hub.Clients.User(userId).SendAsync("ApplicationUpdated", application);
        return Results.Ok(application);
    }

    private static async Task<IResult> DeleteApplication(string id, HttpContext context, IMediator mediator, IHubContext<JobApplicationsHub> hub)
    {
        var userId = context.GetUserId();
        var deleted = await mediator.Send(new DeleteJobApplicationCommand(userId, id));
        if (deleted)
            await hub.Clients.User(userId).SendAsync("ApplicationDeleted", id);
        return deleted ? Results.NoContent() : Results.NotFound();
    }
}

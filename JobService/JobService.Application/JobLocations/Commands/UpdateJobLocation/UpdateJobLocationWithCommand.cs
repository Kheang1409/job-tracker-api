using MediatR;

namespace JobTracker.JobService.Application.JobLocations.Commands.UpdateJobLocation;

public record UpdateJobLocationWithIdCommand(
    string AuthorId,
    string JobPostId,
    string Address,
    int PostalCode,
    string City,
    string County,
    string State,
    string Country) : IRequest<bool>;
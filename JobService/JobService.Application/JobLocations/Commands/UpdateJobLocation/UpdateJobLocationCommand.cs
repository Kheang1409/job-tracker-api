using MediatR;

namespace JobTracker.JobService.Application.JobLocations.Commands.UpdateJobLocation;

public record UpdateJobLocationCommand(
    string Address,
    int PostalCode,
    string City,
    string County,
    string State,
    string Country) : IRequest<bool>;
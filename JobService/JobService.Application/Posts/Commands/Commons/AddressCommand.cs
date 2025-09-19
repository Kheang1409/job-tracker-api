namespace JobTracker.JobService.Application.Posts.Commands.Commons;

public record AddressCommand(
    string Country,
    string State,
    string City,
    string Street,
    int PostalCode
);

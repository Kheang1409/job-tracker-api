using MediatR;

namespace JobTracker.UserService.Application.Addresses.Commands.CreateAddress;

public record CreateAddressWithIdCommand(
    string UserId,
    string Address1,
    string Address2,
    int PostalCode,
    string City,
    string County,
    string State,
    string Country) : IRequest<string>;
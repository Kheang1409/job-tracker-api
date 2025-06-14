using MediatR;

namespace JobTracker.UserService.Application.Addresses.Commands.CreateAddress;

public record CreateAddressCommand(
    string Address1,
    string Address2,
    int PostalCode,
    string City,
    string County,
    string State,
    string Country) : IRequest<string>;
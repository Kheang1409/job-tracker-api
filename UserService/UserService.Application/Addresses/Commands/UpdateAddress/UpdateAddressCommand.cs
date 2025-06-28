using MediatR;

namespace JobTracker.UserService.Application.Addresses.Commands.UpdateAddress;

public record UpdateAddressCommand(
    string Address1,
    string Address2,
    int PostalCode,
    string City,
    string County,
    string State,
    string Country) : IRequest<bool>;
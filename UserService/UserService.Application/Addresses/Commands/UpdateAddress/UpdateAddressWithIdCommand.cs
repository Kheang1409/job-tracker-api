using MediatR;

namespace JobTracker.UserService.Application.Addresses.Commands.UpdateAddress;

public record UpdateAddressWithIdCommand(
    string UserId,
    string AddressId,
    string Address1,
    string Address2,
    int PostalCode,
    string City,
    string County,
    string State,
    string Country) : IRequest<bool>;
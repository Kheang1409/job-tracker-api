using MediatR;

namespace JobTracker.UserService.Application.Addresses.Commands.DeleteAddress;

public record DeleteAddressCommand(string UserId, string AddressId) : IRequest<bool>;
using JobTracker.UserService.Domain.Entities;
using MediatR;

namespace JobTracker.UserService.Application.Addresses.Queries.GetAddresses;

public record GetAddressesQuery(string UserId) : IRequest<IEnumerable<Address>>;
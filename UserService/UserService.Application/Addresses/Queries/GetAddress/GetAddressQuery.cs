using JobTracker.UserService.Domain.Entities;
using MediatR;

namespace JobTracker.UserService.Application.Addresses.Queries.GetAddress;

public record GetAddressQuery(string UserId, string AddressId) : IRequest<Address>;
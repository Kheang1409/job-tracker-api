using JobTracker.UserService.Domain.Entities;
using MediatR;

namespace JobTracker.UserService.Application.Users.Queries.GetUserProfile;

public record GetUserProfileQuery(string Id) : IRequest<User>;
using JobTracker.UserService.Domain.Entities;
using MediatR;

namespace JobTracker.UserService.Application.Users.Queries.GetUsers;

public record GetUsersQuery(string? Fullname, string? Skill, int PageNumber=1, int Limit=10) : IRequest<IEnumerable<User>>;
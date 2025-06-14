using JobTracker.UserService.Domain.Entities;
using MediatR;

namespace JobTracker.UserService.Application.Projects.Queries.GetProjects;

public record GetProjectsQuery(string UserId) : IRequest<IEnumerable<Project>>;
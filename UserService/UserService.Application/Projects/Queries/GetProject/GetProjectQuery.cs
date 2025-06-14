using JobTracker.UserService.Domain.Entities;
using MediatR;

namespace JobTracker.UserService.Application.Projects.Queries.GetProject;

public record GetProjectQuery(string UserId, string ProjectId) : IRequest<Project>;
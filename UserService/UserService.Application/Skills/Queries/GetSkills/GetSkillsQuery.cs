using JobTracker.UserService.Domain.Entities;
using MediatR;

namespace JobTracker.UserService.Application.Skills.Queries.GetSkills;

public record GetSkillsQuery(string UserId) : IRequest<IEnumerable<Skill>>;
using JobTracker.UserService.Domain.Entities;
using MediatR;

namespace JobTracker.UserService.Application.Skills.Queries.GetSkill;

public record GetSkillQuery(string UserId, string SkillId) : IRequest<Skill>;
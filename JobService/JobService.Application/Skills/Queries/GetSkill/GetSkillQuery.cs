using JobTracker.JobService.Domain.Entities;
using MediatR;

namespace JobTracker.JobService.Application.Skills.Queries.GetSkill;

public record GetSkillQuery(string JobPostId, string SkillId) : IRequest<Skill>;
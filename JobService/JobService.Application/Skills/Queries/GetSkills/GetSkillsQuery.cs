using JobTracker.JobService.Domain.Entities;
using MediatR;

namespace JobTracker.JobService.Application.Skills.Queries.GetSkills;

public record GetSkillsQuery(string JobPostId) : IRequest<IEnumerable<Skill>>;
using MediatR;

namespace JobTracker.JobService.Application.Skills.Commands.DeleteSkill;


public record DeleteSkillCommand(string UserId, string JobPostId, string SkillId) : IRequest<bool>;
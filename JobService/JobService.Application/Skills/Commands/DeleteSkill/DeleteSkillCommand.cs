using MediatR;

namespace JobTracker.JobService.Application.Skills.Commands.DeleteSkill;


public record DeleteSkillCommand(string AuthorId, string JobPostId, string SkillId) : IRequest<bool>;
using MediatR;

namespace JobTracker.JobService.Application.Skills.Commands.CreateSkill;


public record CreateSkillWithIdCommand( string AuthorId, string JobPostId, string Name) : IRequest<string>;
using MediatR;

namespace JobTracker.JobService.Application.Skills.Commands.CreateSkill;

public record CreateSkillCommand(string Name) : IRequest<string>;
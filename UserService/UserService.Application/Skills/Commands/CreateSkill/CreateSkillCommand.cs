using MediatR;

namespace JobTracker.UserService.Application.Skills.Commands.CreateSkill;

public record CreateSkillCommand(string Name) : IRequest<string>;
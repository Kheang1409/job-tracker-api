using MediatR;

namespace JobTracker.UserService.Application.Skills.Commands.UpdateSkill;


public record UpdateSkillCommand(string Name) : IRequest<bool>;
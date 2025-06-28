using MediatR;

namespace JobTracker.UserService.Application.Skills.Commands.CreateSkill;


public record CreateSkillWithIdCommand(string UserId, string Name) : IRequest<string>;
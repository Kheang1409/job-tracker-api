using MediatR;

namespace JobTracker.UserService.Application.Skills.Commands.UpdateSkill;


public record UpdateSkillWithIdCommand(string UserId, string SkillId, string Name) : IRequest<bool>;
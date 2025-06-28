using MediatR;

namespace JobTracker.UserService.Application.Skills.Commands.DeleteSkill;


public record DeleteSkillCommand(string UserId, string SkillId) : IRequest<bool>;
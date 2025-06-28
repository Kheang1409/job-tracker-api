using JobTracker.UserService.Application.Repositories;
using JobTracker.UserService.Domain.Entities;
using MediatR;

namespace JobTracker.UserService.Application.Skills.Commands.CreateSkill;
public class CreateSkillCommandHandler : IRequestHandler<CreateSkillWithIdCommand, string>
{
    private readonly ISkillRepository _skillRepository;
    public CreateSkillCommandHandler(
        ISkillRepository skillRepository
    )
    {
        _skillRepository = skillRepository;
    }
    
    public async Task<string> Handle(CreateSkillWithIdCommand command, CancellationToken cancellationToken)
    {
        var skill = Skill.Create(command.Name);
        var skillId = await _skillRepository.AddAsync(command.UserId, skill);
        return skillId;
    }
}
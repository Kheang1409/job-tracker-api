using JobTracker.UserService.Application.Repositories;
using MediatR;

namespace JobTracker.UserService.Application.Skills.Commands.UpdateSkill;
public class UpdateSkillCommandHandler : IRequestHandler<UpdateSkillWithIdCommand, bool>
{
    private readonly ISkillRepository _skillRepository;
    public UpdateSkillCommandHandler(
        ISkillRepository skillRepository
    )
    {
        _skillRepository = skillRepository;
    }
    
    public async Task<bool> Handle(UpdateSkillWithIdCommand command, CancellationToken cancellationToken)
    {
        var skill = await _skillRepository.GetByIdAsync(command.UserId, command.SkillId);
        skill.Rename(command.Name);
        return await _skillRepository.UpdateAsync(command.UserId, skill);
    }
}
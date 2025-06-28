using JobTracker.JobService.Application.Repositories;
using JobTracker.JobService.Domain.Entities;
using MediatR;

namespace JobTracker.JobService.Application.Skills.Queries.GetSkill;

public class GetSkillQueryHandler : IRequestHandler<GetSkillQuery, Skill>
{
    private readonly ISkillRepository _skillRepository;

    public GetSkillQueryHandler(
        ISkillRepository skillRepository)
    {
        _skillRepository = skillRepository;
    }
    
    public async Task<Skill> Handle(GetSkillQuery command, CancellationToken cancellationToken)
    {
        var skill = await _skillRepository.GetByIdAsync(command.JobPostId, command.SkillId);
        if(skill is null)
            throw new InvalidOperationException($"The skill is not exits.");
        return skill;
    }
}
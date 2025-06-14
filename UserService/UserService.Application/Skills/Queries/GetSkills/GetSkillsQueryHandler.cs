using JobTracker.UserService.Application.Repositories;
using JobTracker.UserService.Domain.Entities;
using MediatR;

namespace JobTracker.UserService.Application.Skills.Queries.GetSkills;

public class GetSkillsQueryHandler : IRequestHandler<GetSkillsQuery, IEnumerable<Skill>>
{
    private readonly ISkillRepository _skillRepository;

    public GetSkillsQueryHandler(
        ISkillRepository skillRepository)
    {
        _skillRepository = skillRepository;
    }
    
    public async Task<IEnumerable<Skill>> Handle(GetSkillsQuery command, CancellationToken cancellationToken)
    {
        var skills = await _skillRepository.GetAllAsync(command.UserId);
        return skills;
    }
}
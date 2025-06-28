using JobTracker.JobService.Domain.Entities;

namespace JobTracker.JobService.Application.Repositories;

public interface ISkillRepository
{
    Task<Skill> GetByIdAsync(string JobPostId, string SkillId);
    Task<IEnumerable<Skill>> GetAllAsync(string JobPostId);
    Task<string> AddAsync(string UserId, string JobPostId, Skill Skill);
    Task<bool> UpdateAsync(string UserId, string JobPostId, Skill Skill);
    Task<bool> DeleteAsync(string UserId, string JobPostId, string SkillId);
}


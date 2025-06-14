using JobTracker.UserService.Domain.Entities;

namespace JobTracker.UserService.Application.Repositories;

public interface ISkillRepository
{
    Task<Skill> GetByIdAsync(string UserId, string SkillId);
    Task<IEnumerable<Skill>> GetAllAsync(string UserId);
    Task<string> AddAsync(string UserId, Skill Skill);
    Task<bool> UpdateAsync(string UserId, Skill Skill);
    Task<bool> DeleteAsync(string UserId, string SkillId);
}


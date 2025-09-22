using JobTracker.UserService.Domain.Entities;

namespace JobTracker.UserService.Application.Repositories;

public interface IUserRepository
{
    Task<User> GetByIdAsync(string Id);
    Task<User> GetByEmailAsync(string Email);
    Task<IEnumerable<User>> GetAllAsync(string Fullname, string Skill, int PageNumber, int Limit);
    Task<int> GetUserCountAsync(string Fullname, string Skill);
    Task<string> AddAsync(User User);
    Task<bool> UpdateAsync(User User);
    Task<bool> DeleteAsync(string Id);
}
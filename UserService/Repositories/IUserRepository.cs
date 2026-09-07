using JobTracker.UserService.Domain.Entities;
using JobTracker.UserService.Application.Services;

namespace JobTracker.UserService.Application.Repositories;

public interface IUserRepository
{
    Task<User> GetByIdAsync(string Id);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByUsernameAsync(string username);
    Task<IEnumerable<User>> GetAllAsync(string Fullname, string Skill, int PageNumber, int Limit);
    Task<int> GetUserCountAsync(string Fullname, string Skill);
    Task<string> AddAsync(User User);
    Task<string> AddWithEmailAsync(User user, EmailOutboxMessage email, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(User User);
    Task<bool> UpdateWithEmailAsync(User user, EmailOutboxMessage email, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(string Id);
}

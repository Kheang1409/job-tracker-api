using JobTracker.UserService.Domain.Entities;

namespace JobTracker.UserService.Application.Repositories;
public interface IUserRepository
{
    Task<User> GetByIdAsync(string Id);
    Task<User> GetByEmailAsync(string Email);
    Task<User> GetByOTPAsync(string OTP);
    Task<IEnumerable<User>> GetAllAsync(string Fullname, string Skill, int PageNumber, int Limit);
    Task<string> AddAsync(User User);
    Task<bool> UpdateAsync(User User);
    Task<bool> DeleteAsync(string Id);
}
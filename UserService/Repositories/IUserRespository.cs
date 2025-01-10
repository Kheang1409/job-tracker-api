using UserService.Models;

namespace UserService.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByOPTAsync(string opt);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task UpdateOPTAsync(string email, string opt, DateTime expiry);
    }
}

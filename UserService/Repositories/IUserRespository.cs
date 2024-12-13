using UserService.Models;

namespace UserService.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByResetTokenAsync(string resetToken);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task UpdateResetTokenAsync(string email, string resetToken, DateTime expiry);

    }
}

using JobTracker.UserService.Domain.Entities;
using JobTracker.UserService.Domain.Factories;
using JobTracker.UserService.Domain.Enum;

namespace JobTracker.UserService.Infrastructure.Factories;

public class UserFactory : IUserFactory
{
    public User CreateNormalUser(string email, string password)
    {
        return User.Create(email, password, UserRole.Normal_User);
    }
    public User CreateAdminUser(string email, string password)
    {
        return User.Create(email, password, UserRole.Admin);
    }
     
}
using JobTracker.UserService.Domain.Entities;
using JobTracker.UserService.Domain.Factories;
using JobTracker.UserService.Domain.Enum;

namespace JobTracker.UserService.Infrastructure.Factories;

public class UserFactory : IUserFactory
{
    public User CreateNormalUser(string firstName, string lastName, string email, string password)
    {
        return User.Create(firstName, lastName, email, password, UserRole.Normal_User);
    }
    public User CreateAdminUser(string firstName, string lastName, string email, string password)
    {
        return User.Create(firstName, lastName, email, password, UserRole.Admin);
    }
     
}
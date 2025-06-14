using JobTracker.UserService.Domain.Entities;

namespace JobTracker.UserService.Domain.Factories;

public interface IUserFactory
{
    User CreateNormalUser(string email, string password);
    User CreateAdminUser(string email, string password);
}
using JobTracker.UserService.Domain.Entities;

namespace JobTracker.UserService.Domain.Factories;

public interface IUserFactory
{
    User CreateNormalUser(string firstName, string lastName, string email, string password);
    User CreateAdminUser(string firstName, string lastName, string email, string password);
}
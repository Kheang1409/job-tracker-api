using JobTracker.UserService.Domain.Entities;

namespace JobTracker.UserService.Domain.Factories;

public interface IUserFactory
{
    User Create(string username, string firstName, string lastName, string email, string password);
}
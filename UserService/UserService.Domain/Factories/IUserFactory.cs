using JobTracker.UserService.Domain.Entities;

namespace JobTracker.UserService.Domain.Factories;

public interface IUserFactory
{
    User Create(string firstName, string lastName, string email, string password);
}
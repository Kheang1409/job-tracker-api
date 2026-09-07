using JobTracker.UserService.Domain.Entities;
using JobTracker.UserService.Domain.Factories;

namespace JobTracker.UserService.Infrastructure.Factories;

public class UserFactory : IUserFactory
{
    public User Create(string username, string firstName, string lastName, string email, string password)
    {
        return new User.Builder()
                .SetUsername(username)
                .SetFirstName(firstName)
                .SetLastName(lastName)
                .SetEmail(email)
                .SetPassword(password)
                .Build();
    }
}
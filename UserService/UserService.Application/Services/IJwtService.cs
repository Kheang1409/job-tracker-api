using JobTracker.UserService.Domain.Entities;

namespace JobTracker.UserService.Application.Services;
public interface IJwtService
{
    Task<string> GenerateToken(User user);
}
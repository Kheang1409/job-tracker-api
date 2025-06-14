using JobTracker.UserService.Domain.Entities;

namespace JobTracker.UserService.Application.Repositories;

public interface IProjectRepository
{
    Task<Project> GetByIdAsync(string UserId, string ProjectId);
    Task<IEnumerable<Project>> GetAllAsync(string UserId);
    Task<string> AddAsync(string UserId, Project Project);
    Task<bool> UpdateAsync(string UserId, Project Project);
    Task<bool> DeleteAsync(string UserId, string ProjectId);
}


using JobTracker.JobService.Domain.Entities;

namespace JobTracker.JobService.Application.Repositories;
public interface IPostRepository
{
    Task<int> GetPostCountAsync(string Title, string CompanyName, string AuthorId);
    Task<Post> GetByIdAsync(string Id);
    Task<IEnumerable<Post>> GetAllAsync(string Title, string CompanyName, string AuthorId, int PageNumber, int Limit);
    Task<string> AddAsync(Post post);
    Task<bool> UpdateAsync(Post post);
    Task<bool> DeleteAsync(string Id);
}
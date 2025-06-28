using JobTracker.JobService.Domain.Entities;

namespace JobTracker.JobService.Application.Repositories;

public interface ICandidateRepository
{
    Task<Candidate> GetByIdAsync(string JobPostId, string CandidateId);
    Task<IEnumerable<Candidate>> GetAllAsync(string JobPostId, int PageNumber, int Limit);
    Task<string> AddAsync(string JobPostId, Candidate Candidate);
    Task<bool> UpdateAsync(string JobPostId, string CandidateId, Candidate Candidate);
}
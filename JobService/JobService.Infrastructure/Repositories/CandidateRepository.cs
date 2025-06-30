using JobTracker.JobService.Application.Repositories;
using JobTracker.JobService.Domain.Entities;
using JobTracker.JobService.Domain.Enums;
using JobTracker.SharedKernel.Exceptions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace JobTracker.JobService.Infrastructure.Repositories;

public class CandidateRepository : ICandidateRepository
{
    private readonly IMongoCollection<JobPosting> _jobPosts;
    public CandidateRepository(
        IMongoClient mongoClient
        )
    {
        var database = mongoClient.GetDatabase("JobTrackerApp");
        _jobPosts = database.GetCollection<JobPosting>("JobPostings");
    }

    public async Task<Candidate> GetByIdAsync(string JobPostId, string CandidateId)
    {
        var filter = Builders<JobPosting>.Filter.Eq(j => j.Id, JobPostId);
        var projection = Builders<JobPosting>.Projection
            .ElemMatch(j => j.Candidates, c => c.CandidateId == CandidateId && c.Status == ApplicationStatus.Applied)
            .Exclude("_id");

        var result = await _jobPosts.Find(filter)
            .Project<JobPostingProjection>(projection)
            .FirstOrDefaultAsync();

        if (result == null || result.Candidates == null || !result.Candidates.Any())
            throw new NotFoundException("Candidate not found");
        return result.Candidates.First();
    }

    public async Task<IEnumerable<Candidate>> GetAllAsync(string JobPostId, int PageNumber, int Limit)
    {
        var JobPostingFilter = Builders<JobPosting>.Filter.Eq(j => j.Id, JobPostId);
        var projection = Builders<JobPosting>.Projection
            .ElemMatch(j => j.Candidates, c => c.Status == ApplicationStatus.Applied)
            .Exclude("_id");

        var JobPosting = await _jobPosts.Find(JobPostingFilter)
            .Project<JobPostingProjection>(projection)
            .FirstOrDefaultAsync();

        return JobPosting?.Candidates ?? Enumerable.Empty<Candidate>();
    }
    public async Task<string> AddAsync(string JobPostId, Candidate Candidate)
    {
        var filter = Builders<JobPosting>.Filter.And(
            Builders<JobPosting>.Filter.Eq(j => j.Id, JobPostId)
        );
        var addCandidate = Builders<JobPosting>.Update.Push(j => j.Candidates, Candidate);
        var result = await _jobPosts.UpdateOneAsync(filter, addCandidate);
        if (result.MatchedCount == 0)
            throw new NotFoundException("JobPosting not found");
        return Candidate.CandidateId;
    }
    public async Task<bool> UpdateAsync(string JobPostId, string CandidateId, Candidate Candidate)
    {
        var filter = Builders<JobPosting>.Filter.And(
            Builders<JobPosting>.Filter.Eq(j => j.Id, JobPostId),
            Builders<JobPosting>.Filter.ElemMatch(j => j.Candidates, c => c.CandidateId == Candidate.CandidateId && c.Status == ApplicationStatus.Applied)
        );
        Console.WriteLine($"Dude: {Candidate.ToJson()}");
        var update = Builders<JobPosting>.Update
            .Set("Candidates.$.Status", Candidate.Status)
            .Set("Candidates.$.Rounds", Candidate.Rounds);

        var result = await _jobPosts.UpdateOneAsync(filter, update);
        if (result.MatchedCount == 0)
            throw new NotFoundException("JobPosting or Candidate not found");
        return true;
    }
    
    private class JobPostingProjection
    {
        public List<Candidate>? Candidates { get; set; }
    }
}
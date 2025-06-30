using JobTracker.JobService.Application.Repositories;
using JobTracker.JobService.Domain.Entities;
using JobTracker.JobService.Domain.Enums;
using JobTracker.SharedKernel.Exceptions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace JobTracker.JobService.Infrastructure.Repositories;
public class JobPostRepository : IJobPostRepository
{
    private readonly IMongoCollection<JobPosting> _jobPosts;

    public JobPostRepository(IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase("JobTrackerApp");
        _jobPosts = database.GetCollection<JobPosting>("JobPostings");
    }
    public async Task<JobPosting> GetByIdAsync(string Id)
    {
        var jobPost = await _jobPosts.Find(u => u.Id == Id).SingleOrDefaultAsync();
        if (jobPost is null)
            throw new NotFoundException($"The Job Post unavaliable.");
        return jobPost;
    }

    public async Task<int> GetJobCountAsync(string Title, string CompanyName,  string AuthorId, string CandidateId)
    {
        var filterBuilder = Builders<JobPosting>.Filter;
        var filters = new List<FilterDefinition<JobPosting>>();

        if (!string.IsNullOrWhiteSpace(Title))
        {
            filters.Add(filterBuilder.Regex(u => u.Title, new BsonRegularExpression(Title, "i")));
        }
        if (!string.IsNullOrWhiteSpace(CompanyName))
        {
            filters.Add(filterBuilder.Regex(u => u.CompanyName, new BsonRegularExpression(CompanyName, "i")));
        }
        if (!string.IsNullOrWhiteSpace(AuthorId))
        {
            filters.Add(filterBuilder.Eq(u => u.AuthorId, AuthorId));
        }
        if (!string.IsNullOrWhiteSpace(CandidateId))
        {
            filters.Add(filterBuilder.ElemMatch(u => u.Candidates, c => c.CandidateId == CandidateId && c.Status == ApplicationStatus.Applied));
        }

        var combinedFilter = filters.Count > 0 ? filterBuilder.And(filters) : FilterDefinition<JobPosting>.Empty;

        var count = await _jobPosts.CountDocumentsAsync(combinedFilter);
        return (int)count;
    }
    
    public async Task<IEnumerable<JobPosting>> GetAllAsync(string Title, string CompanyName, string AuthorId, string CandidateId, int PageNumber, int Limit)
    {
        var filterBuilder = Builders<JobPosting>.Filter;
        var filters = new List<FilterDefinition<JobPosting>>();

        if (!string.IsNullOrWhiteSpace(Title))
        {
            filters.Add(filterBuilder.Regex(u => u.CompanyName, Title));
        }
        if (!string.IsNullOrWhiteSpace(CompanyName))
        {
            filters.Add(filterBuilder.Regex(u => u.CompanyName, CompanyName));
        }
        if (!string.IsNullOrWhiteSpace(AuthorId))
        {
            filters.Add(filterBuilder.Eq(u => u.AuthorId, AuthorId));
        }
        if (!string.IsNullOrWhiteSpace(CandidateId))
        {
            filters.Add(filterBuilder.ElemMatch(u => u.Candidates, c => c.CandidateId == CandidateId && c.Status == ApplicationStatus.Applied));
        }

        var combinedFilter = filters.Count > 0 ? filterBuilder.And(filters) : FilterDefinition<JobPosting>.Empty;

        var skip = (PageNumber - 1) * Limit;

        var jobPosts = await _jobPosts.Find(combinedFilter)
                                    .Skip(skip)
                                    .Limit(Limit)
                                    .ToListAsync();
        return jobPosts;
    }
    public async Task<string> AddAsync(JobPosting JobPosting)
    {
        await _jobPosts.InsertOneAsync(JobPosting);
        return JobPosting.Id;
    }
    public async Task<bool> UpdateAsync(JobPosting JobPosting)
    {
        var result = await _jobPosts.ReplaceOneAsync(u => u.Id == JobPosting.Id, JobPosting);
        if (result.MatchedCount == 0)
            throw new NotFoundException($"Job Posting with Id '{JobPosting.Id}' not found.");
        return true;
    }
    public async Task<bool> DeleteAsync(string Id)
    {
        var deletedJob = await _jobPosts.FindOneAndDeleteAsync(u => u.Id == Id);
        return deletedJob != null;
    }
}
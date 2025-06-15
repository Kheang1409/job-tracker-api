using JobTracker.JobService.Application.CustomExceptions;
using JobTracker.JobService.Application.Repositories;
using JobTracker.JobService.Domain.Entities;
using MongoDB.Driver;

namespace JobTracker.JobService.Infrastructure.Repositories;

public class SkillRepository : ISkillRepository
{

    private readonly IJobPostRepository _jobPostRepository;
    private readonly IMongoCollection<JobPosting> _jobPosts;

    public SkillRepository(
        IJobPostRepository jobPostRepository,
        IMongoClient mongoClient
    )
    {
        _jobPostRepository = jobPostRepository;
        var database = mongoClient.GetDatabase("JobTrackerApp");
        _jobPosts = database.GetCollection<JobPosting>("JobPostings");
    }

    public async Task<Skill> GetByIdAsync(string JobPostId, string SkillId)
    {
        var filter = Builders<JobPosting>.Filter.Eq(j => j.Id, JobPostId);
        var projection = Builders<JobPosting>.Projection
            .ElemMatch(j => j.RequiredSkills, s => s.Id == SkillId)
            .Exclude("_id");

        var result = await _jobPosts.Find(filter)
            .Project<JobPostingProjection>(projection)
            .FirstOrDefaultAsync();

        if (result == null || result.RequiredSkills == null || !result.RequiredSkills.Any())
            throw new NotFoundException("Skill not found");
        return result.RequiredSkills.First();
    }
    public async Task<IEnumerable<Skill>> GetAllAsync(string JobPostId)
    {
        var JobPostingFilter = Builders<JobPosting>.Filter.Eq(j => j.Id, JobPostId);
        var JobPosting = await _jobPosts.Find(JobPostingFilter).FirstOrDefaultAsync();
        return JobPosting?.RequiredSkills ?? Enumerable.Empty<Skill>();
    }

    public async Task<string> AddAsync(string UserId, string JobPostId, Skill Skill)
    {
        var jobPosting = await _jobPostRepository.GetByIdAsync(JobPostId);
        if(jobPosting.AutherId != UserId)
            throw new UnauthorizedAccessException("User is not authorized to modify this job posting");

        var filter = Builders<JobPosting>.Filter.And(
            Builders<JobPosting>.Filter.Eq(j => j.Id, JobPostId)
        );
        var addSkill = Builders<JobPosting>.Update.Push(j => j.RequiredSkills, Skill);
        var result = await _jobPosts.UpdateOneAsync(filter, addSkill);
        if (result.MatchedCount == 0)
            throw new NotFoundException("JobPosting not found");
        return Skill.Id;
    }

    public async Task<bool> UpdateAsync(string UserId, string JobPostId, Skill Skill)
    {
        var jobPosting = await _jobPostRepository.GetByIdAsync(JobPostId);
        if(jobPosting.AutherId != UserId)
            throw new UnauthorizedAccessException("User is not authorized to modify this job posting");

        var filter = Builders<JobPosting>.Filter.And(
            Builders<JobPosting>.Filter.Eq(j => j.Id, JobPostId),
            Builders<JobPosting>.Filter.ElemMatch(j => j.RequiredSkills, s => s.Id == Skill.Id)
        );

        var update = Builders<JobPosting>.Update
            .Set(j => j.RequiredSkills[0].Name, Skill.Name);

        var result = await _jobPosts.UpdateOneAsync(filter, update);
        if (result.MatchedCount == 0)
            throw new NotFoundException("JobPosting or Skill not found");
        return true;
    }

    public async Task<bool> DeleteAsync(string UserId, string JobPostId, string SkillId)
    {
        var jobPosting = await _jobPostRepository.GetByIdAsync(JobPostId);
        if(jobPosting.AutherId != UserId)
            throw new UnauthorizedAccessException("User is not authorized to modify this job posting");

        var filter = Builders<JobPosting>.Filter.Eq(u => u.Id, JobPostId);
        var update = Builders<JobPosting>.Update.PullFilter(u => u.RequiredSkills, s => s.Id == SkillId);
        var result = await _jobPosts.UpdateOneAsync(filter, update);
        if (result.MatchedCount == 0)
            throw new NotFoundException("JobPosting or Skill not found");
        return true;
    }

    private class JobPostingProjection
    {
        public List<Skill>? RequiredSkills { get; set; }
    }
}
using JobTracker.JobService.Application.Repositories;
using JobTracker.JobService.Domain.Entities;
using JobTracker.SharedKernel.Exceptions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace JobTracker.JobService.Infrastructure.Repositories;
public class JobRepository : IPostRepository
{
    private readonly IMongoCollection<Post> _posts;

    public JobRepository(IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase("JobTrackerApp");
        _posts = database.GetCollection<Post>("Posts");
    }
    public async Task<Post> GetByIdAsync(string Id)
    {
        var jobPost = await _posts.Find(u => u.Id == Id).SingleOrDefaultAsync();
        if (jobPost is null)
            throw new NotFoundException($"The Job Post unavaliable.");
        return jobPost;
    }

    public async Task<int> GetPostCountAsync(
        string Title,
        string CompanyName,
        string AuthorId
    )
    {
        var filterBuilder = Builders<Post>.Filter;

        var andFilters = new List<FilterDefinition<Post>>();

        if (!string.IsNullOrWhiteSpace(Title))
        {
            andFilters.Add(filterBuilder.Regex(u => u.Title, new BsonRegularExpression(Title, "i")));
        }
        if (!string.IsNullOrWhiteSpace(CompanyName))
        {
            andFilters.Add(filterBuilder.Regex(u => u.CompanyName, new BsonRegularExpression(CompanyName, "i")));
        }

        var orFilters = new List<FilterDefinition<Post>>();

        if (!string.IsNullOrWhiteSpace(AuthorId))
        {
            orFilters.Add(filterBuilder.Eq(u => u.AuthorId, AuthorId));
        }

        if (orFilters.Count > 0)
        {
            andFilters.Add(filterBuilder.Or(orFilters));
        }

        var finalFilter = andFilters.Count > 0
            ? filterBuilder.And(andFilters)
            : FilterDefinition<Post>.Empty;

        var count = await _posts.CountDocumentsAsync(finalFilter);
        return (int)count;
    }


    
    public async Task<IEnumerable<Post>> GetAllAsync(
        string Title,
        string CompanyName,
        string AuthorId,
        int PageNumber,
        int Limit
    )
    {
        var filterBuilder = Builders<Post>.Filter;

        var andFilters = new List<FilterDefinition<Post>>();

        if (!string.IsNullOrWhiteSpace(Title))
        {
            andFilters.Add(filterBuilder.Regex(u => u.Title, new BsonRegularExpression(Title, "i")));
        }
        if (!string.IsNullOrWhiteSpace(CompanyName))
        {
            andFilters.Add(filterBuilder.Regex(u => u.CompanyName, new BsonRegularExpression(CompanyName, "i")));
        }

        var orFilters = new List<FilterDefinition<Post>>();

        if (!string.IsNullOrWhiteSpace(AuthorId))
        {
            orFilters.Add(filterBuilder.Eq(u => u.AuthorId, AuthorId));
        }

        if (orFilters.Count > 0)
        {
            andFilters.Add(filterBuilder.Or(orFilters));
        }

        var finalFilter = andFilters.Count > 0
            ? filterBuilder.And(andFilters)
            : FilterDefinition<Post>.Empty;

        var skip = (PageNumber - 1) * Limit;

        var jobPosts = await _posts.Find(finalFilter)
                                    .Skip(skip)
                                    .Limit(Limit)
                                    .ToListAsync();

        return jobPosts;
    }


    public async Task<string> AddAsync(Post Post)
    {
        await _posts.InsertOneAsync(Post);
        return Post.Id;
    }
    public async Task<bool> UpdateAsync(Post Post)
    {
        var result = await _posts.ReplaceOneAsync(u => u.Id == Post.Id, Post);
        if (result.MatchedCount == 0)
            throw new NotFoundException($"Job Posting with Id '{Post.Id}' not found.");
        return true;
    }
    public async Task<bool> DeleteAsync(string Id)
    {
        var deletedJob = await _posts.FindOneAndDeleteAsync(u => u.Id == Id);
        return deletedJob != null;
    }
}
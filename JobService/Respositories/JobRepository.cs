using JobService.Models;
using MongoDB.Driver;

namespace JobService.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly IMongoCollection<Job> _jobsCollection;

        public JobRepository(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("JobTrackerApp");
            _jobsCollection = database.GetCollection<Job>("Jobs");
        }

        public async Task<IEnumerable<Job>> GetJobsAsync(int pageNumber, string status, string sort)
        {
            var limit = 5;
            var skip = (pageNumber - 1) * limit;
            var filter = Builders<Job>.Filter.Empty;
            if (!string.IsNullOrEmpty(status))
            {
                filter = Builders<Job>.Filter.Eq(job => job.Status, status);
            }
            var sortDefinition = sort switch
            {
                "asc" => Builders<Job>.Sort.Ascending(job => job.CreatedDate),
                "desc" => Builders<Job>.Sort.Descending(job => job.CreatedDate),
                _ => Builders<Job>.Sort.Descending(job => job.CreatedDate)
            };
            return await _jobsCollection.Find(filter).Sort(sortDefinition).Skip(skip).Limit(limit).ToListAsync();
        }

        public async Task<Job> GetJobByIdAsync(string id)
        {
            return await _jobsCollection.Find(j => j.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> GetTotalJobsCountAsync(string status)
        {
            var filter = Builders<Job>.Filter.Empty;
            if (!string.IsNullOrEmpty(status))
            {
                filter = Builders<Job>.Filter.Eq(job => job.Status, status);
            }
            var jobsTotal = await _jobsCollection.Find(filter).CountDocumentsAsync();

            return (int)jobsTotal;
        }

        public async Task CreateJobAsync(Job job)
        {
            await _jobsCollection.InsertOneAsync(job);
        }

        public async Task UpdateJobAsync(Job job)
        {
            await _jobsCollection.ReplaceOneAsync(j => j.Id == job.Id, job);
        }

        public async Task DeleteJobAsync(string id)
        {
            await _jobsCollection.DeleteOneAsync(j => j.Id == id);
        }
    }
}

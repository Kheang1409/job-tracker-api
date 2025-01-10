using UserService.Models;
using MongoDB.Driver;

namespace UserService.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("JobTrackerApp");
            _users = database.GetCollection<User>("Users");

            var indexKeys = Builders<User>.IndexKeys.Ascending(u => u.Email);
            var indexOptions = new CreateIndexOptions { Unique = true };
            _users.Indexes.CreateOne(new CreateIndexModel<User>(indexKeys, indexOptions));
        }

        public async Task<User?> GetByEmailAsync(string email) =>
            await _users.Find(u => u.Email == email).FirstOrDefaultAsync();

        public async Task<User?> GetByOPTAsync(string opt) =>
            await _users.Find(u => u.OPT == opt && u.OPTExpiry > DateTime.UtcNow)
                        .FirstOrDefaultAsync();

        public async Task AddUserAsync(User user) =>
            await _users.InsertOneAsync(user);

        public async Task UpdateUserAsync(User user) =>
            await _users.ReplaceOneAsync(u => u.Id == user.Id, user);

        public async Task UpdateOPTAsync(string email, string opt, DateTime expiry)
        {
            var update = Builders<User>.Update
                .Set(u => u.OPT, opt)
                .Set(u => u.OPTExpiry, expiry);

            await _users.UpdateOneAsync(u => u.Email == email, update);
        }
    }
}

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
        }

        public async Task<User?> GetByEmailAsync(string email) =>
            await _users.Find(u => u.Email == email).FirstOrDefaultAsync();

        public async Task<User?> GetByResetTokenAsync(string resetToken) =>
            await _users.Find(u => u.ResetToken == resetToken && u.ResetTokenExpiry > DateTime.UtcNow)
                        .FirstOrDefaultAsync();

        public async Task AddUserAsync(User user) =>
            await _users.InsertOneAsync(user);

        public async Task UpdateUserAsync(User user) =>
            await _users.ReplaceOneAsync(u => u.Id == user.Id, user);

        public async Task UpdateResetTokenAsync(string email, string resetToken, DateTime expiry)
        {
            var update = Builders<User>.Update
                .Set(u => u.ResetToken, resetToken)
                .Set(u => u.ResetTokenExpiry, expiry);

            await _users.UpdateOneAsync(u => u.Email == email, update);
        }
    }
}

using JobTracker.UserService.Application.CustomExceptions;
using JobTracker.UserService.Application.Repositories;
using JobTracker.UserService.Domain.Entities;
using MongoDB.Driver;

namespace JobTracker.UserService.Infrastructure.Repositories;

public class AddressRepository : IAddressRepository
{

    private readonly IMongoCollection<User> _users;

    public AddressRepository(IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase("JobTrackerApp");
        _users = database.GetCollection<User>("Users");
    }
    public async Task<Address> GetByIdAsync(string UserId, string AddressId)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Id, UserId);
        var projection = Builders<User>.Projection
            .ElemMatch(u => u.Addresses, a => a.Id == AddressId)
            .Exclude("_id");

        var result = await _users.Find(filter)
            .Project<UserProjection>(projection)
            .FirstOrDefaultAsync();

        if (result == null || result.Addresses == null || !result.Addresses.Any())
            throw new NotFoundException("Address not found");
        return result.Addresses.First();
    }
    public async Task<IEnumerable<Address>> GetAllAsync(string UserId)
    {
        var userFilter = Builders<User>.Filter.Eq(u => u.Id, UserId);
        var user = await _users.Find(userFilter).FirstOrDefaultAsync();
        return user?.Addresses ?? Enumerable.Empty<Address>();
    }
    public async Task<string> AddAsync(string UserId, Address Address)
    {
        var findUser = Builders<User>.Filter.Eq(u => u.Id, UserId);
        var addAddress = Builders<User>.Update.Push(u => u.Addresses, Address);

        var result = await _users.UpdateOneAsync(findUser, addAddress);
        if (result.MatchedCount == 0)
            throw new NotFoundException("User not found");
        return Address.Id;
    }
    public async Task<bool> UpdateAsync(string UserId, Address Address)
    {        
        var filter = Builders<User>.Filter.And(
            Builders<User>.Filter.Eq(u => u.Id, UserId),
            Builders<User>.Filter.ElemMatch(u => u.Addresses, a => a.Id == Address.Id)
        );

        var update = Builders<User>.Update
            .Set(u => u.Addresses[0].Address1, Address.Address1)
            .Set(u => u.Addresses[0].Address2, Address.Address2)
            .Set(u => u.Addresses[0].PostalCode, Address.PostalCode)
            .Set(u => u.Addresses[0].City, Address.City)
            .Set(u => u.Addresses[0].County, Address.County)
            .Set(u => u.Addresses[0].State, Address.State)
            .Set(u => u.Addresses[0].Country, Address.Country);
        var result = await _users.UpdateOneAsync(filter, update);
        if (result.MatchedCount == 0)
            throw new NotFoundException("User or Address not found");
        return true;
    }
    public async Task<bool> DeleteAsync(string UserId, string AddressId)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Id, UserId);
        var update = Builders<User>.Update.PullFilter(u => u.Addresses, s => s.Id == AddressId);
        var result = await _users.UpdateOneAsync(filter, update);
        if (result.MatchedCount == 0)
            throw new NotFoundException("User or Address not found");
        return true;
    }
    private class UserProjection
    {
        public List<Address>? Addresses { get; set; }
    }
}


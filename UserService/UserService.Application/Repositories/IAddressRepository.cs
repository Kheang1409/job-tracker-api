using JobTracker.UserService.Domain.Entities;

namespace JobTracker.UserService.Application.Repositories;

public interface IAddressRepository
{
    Task<Address> GetByIdAsync(string UserId, string AddressId);
    Task<IEnumerable<Address>> GetAllAsync(string UserId);
    Task<string> AddAsync(string UserId, Address Address);
    Task<bool> UpdateAsync(string UserId, Address Address);
    Task<bool> DeleteAsync(string UserId, string AddressId);
}


using JobTracker.UserService.Application.Repositories;
using JobTracker.UserService.Domain.Entities;
using MediatR;

namespace JobTracker.UserService.Application.Addresses.Queries.GetAddress;

public class GetAddressQueryHandler : IRequestHandler<GetAddressQuery, Address>
{
    private readonly IAddressRepository _addressRepository;

    public GetAddressQueryHandler(
        IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }
    
    public async Task<Address> Handle(GetAddressQuery command, CancellationToken cancellationToken)
    {
        var address = await _addressRepository.GetByIdAsync(command.UserId, command.AddressId);
        if(address is null)
            throw new InvalidOperationException($"The address is not exits.");
        return address;
    }
}
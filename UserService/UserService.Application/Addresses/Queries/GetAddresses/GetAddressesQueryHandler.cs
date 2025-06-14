using JobTracker.UserService.Application.Repositories;
using JobTracker.UserService.Domain.Entities;
using MediatR;

namespace JobTracker.UserService.Application.Addresses.Queries.GetAddresses;

public class GetSkillsQueryHandler : IRequestHandler<GetAddressesQuery, IEnumerable<Address>>
{
    private readonly IAddressRepository _addressRepository;

    public GetSkillsQueryHandler(
        IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }
    
    public async Task<IEnumerable<Address>> Handle(GetAddressesQuery command, CancellationToken cancellationToken)
    {
        var addresses = await _addressRepository.GetAllAsync(command.UserId);
        return addresses;
    }
}
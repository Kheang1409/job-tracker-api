using JobTracker.UserService.Application.Repositories;
using JobTracker.UserService.Domain.Entities;
using MediatR;

namespace JobTracker.UserService.Application.Addresses.Commands.CreateAddress;

public class CreateAddressCommandHandler : IRequestHandler<CreateAddressWithIdCommand, string>
{
    private readonly IAddressRepository _addressRepository;
    public CreateAddressCommandHandler(
        IAddressRepository addressRepository
    )
    {
        _addressRepository = addressRepository;
    }
    
    public async Task<string> Handle(CreateAddressWithIdCommand command, CancellationToken cancellationToken)
    {
        var address = Address.Create(
            command.Address1,
            command.Address2,
            command.PostalCode,
            command.City,
            command.County,
            command.State,
            command.Country
        );
        var addressId = await _addressRepository.AddAsync(command.UserId, address);
        return addressId;
    }
}
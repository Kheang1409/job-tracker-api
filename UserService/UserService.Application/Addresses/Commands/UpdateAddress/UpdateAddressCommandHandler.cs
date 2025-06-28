using JobTracker.UserService.Application.Repositories;
using MediatR;
using MongoDB.Bson;

namespace JobTracker.UserService.Application.Addresses.Commands.UpdateAddress;

public class UpdateAddressCommandHandler : IRequestHandler<UpdateAddressWithIdCommand, bool>
{
    private readonly IAddressRepository _addressRepository;
    public UpdateAddressCommandHandler(
        IAddressRepository addressRepository
    )
    {
        _addressRepository = addressRepository;
    }
    
    public async Task<bool> Handle(UpdateAddressWithIdCommand command, CancellationToken cancellationToken)
    {
        var address = await _addressRepository.GetByIdAsync(command.UserId, command.AddressId);
        address.Update(
            command.Address1,
            command.Address2,
            command.PostalCode,
            command.City,
            command.County,
            command.State,
            command.Country);
        return await _addressRepository.UpdateAsync(command.UserId, address);
    }
}
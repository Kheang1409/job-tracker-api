using JobTracker.UserService.Application.Repositories;
using MediatR;

namespace JobTracker.UserService.Application.Addresses.Commands.DeleteAddress;

public class DeleteAddressCommandHandler : IRequestHandler<DeleteAddressCommand, bool>
{
    private readonly IAddressRepository _addressRepository;
    public DeleteAddressCommandHandler(
        IAddressRepository addressRepository
    )
    {
        _addressRepository = addressRepository;
    }
    
    public async Task<bool> Handle(DeleteAddressCommand command, CancellationToken cancellationToken)
    {
        return await _addressRepository.DeleteAsync(command.UserId, command.AddressId);
    }
}
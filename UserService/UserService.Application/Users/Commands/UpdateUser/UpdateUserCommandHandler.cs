using JobTracker.UserService.Application.Repositories;
using JobTracker.UserService.Application.Services;
using JobTracker.UserService.Domain.Commons;
using JobTracker.UserService.Domain.Entities;
using MediatR;
using MongoDB.Bson;

namespace JobTracker.UserService.Application.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserWithIdCommand, bool>
{
    private readonly IUserRepository _userRepository;
    public UpdateUserCommandHandler(
        IUserRepository userRepository
    )
    {
        _userRepository = userRepository;
    }
    
    public async Task<bool> Handle(UpdateUserWithIdCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(command.Id);
        user.Update(
            command.FirstName,
            command.LastName,
            command.Bio,
            EnumParser.Gender(command.Gender),
            command.Email,
            command.PhoneNumber
        );
        if (user.Address is null)
        {
            var address = Address.Create(
                command.Country,
                command.Street,
                command.City,
                command.State,
                command.PostalCode
            );
            user.SetAddress(address);
        }
        else
        {
            user.Address?.Update(
            command.Country,
            command.Street,
            command.City,
            command.State,
            command.PostalCode);
        }
        
        return await _userRepository.UpdateAsync(user);
    }
}
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
            command.Skills.Select(s => Skill.Create(s.Name)).ToList()
        );
        var address = Address.Create(
                command.Country,
                command.Street,
                command.City,
                command.State,
                command.PostalCode
            );
        var contactNumber = ContactNumber.Create(
            command.CountryCode,
            command.PhoneNumber
            );
            
        user.SetAddress(address);
        user.SetContactNumber(contactNumber);
        
        return await _userRepository.UpdateAsync(user);
    }
}
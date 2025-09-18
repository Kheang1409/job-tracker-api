using JobTracker.UserService.Application.Repositories;
using MediatR;

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
        user.UpdateProfile(
            command.FirstName,
            command.LastName,
            command.Email,
            command.ContactNumber,
            command.Bio,
            command.Skills,
            command.Experiences,
            command.Projects,
            command.Address
        );
        
        return await _userRepository.UpdateAsync(user);
    }
}
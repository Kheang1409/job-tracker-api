using JobTracker.UserService.Application.Repositories;
using JobTracker.UserService.Application.Services;
using MediatR;

namespace JobTracker.UserService.Application.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserWithIdCommand, string>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    public UpdateUserCommandHandler(
        IUserRepository userRepository,
        IJwtService jwtService
    )
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }
    
    public async Task<string> Handle(UpdateUserWithIdCommand command, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetByIdAsync(command.Id);
        existingUser.Update(
            command.Firstname,
            command.Lastname,
            command.Bio,
            command.Gender,
            command.Email,
            command.CountryCode,
            command.PhoneNumber
        );
        var isUpdate = await _userRepository.UpdateAsync(existingUser);
        // if(!isUpdate)
        //     throw new 
        var token = await _jwtService.GenerateToken(existingUser);
        return token;
    }
}
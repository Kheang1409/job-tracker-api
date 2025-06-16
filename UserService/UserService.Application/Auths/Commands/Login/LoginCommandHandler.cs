using JobTracker.UserService.Application.Repositories;
using JobTracker.UserService.Application.Services;
using MediatR;

namespace JobTracker.UserService.Application.Auths.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IJwtService jwtService
    )
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }
    
    public async Task<string> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetByEmailAsync(command.Email);
        if(existingUser is null)
            throw new UnauthorizedAccessException($"Invalid email or password.");
        if (!existingUser.Verify(command.Password))
            throw new UnauthorizedAccessException("Invalid email or password.");
        var token = await _jwtService.GenerateToken(existingUser);
        return token;
    }
}
using JobTracker.SharedKernel.Exceptions;
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
        var user = await _userRepository.GetByEmailAsync(command.Email);
        if(user is null)
            throw new NotFoundException($"Invalid email or password.");
        if (!user.Verify(command.Password))
            throw new UnauthorizedAccessException("Invalid email or password.");
        var token = await _jwtService.GenerateToken(user);
        return token;
    }
}
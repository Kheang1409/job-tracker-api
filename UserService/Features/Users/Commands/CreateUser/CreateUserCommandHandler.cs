using JobTracker.UserService.Application.Repositories;
using JobTracker.UserService.Application.Services;
using JobTracker.UserService.Domain.Factories;
using MediatR;

namespace JobTracker.UserService.Application.Users.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, string>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserFactory _userFactory;
    public CreateUserCommandHandler(
        IUserRepository userRepository,
        IUserFactory userFactory)
    {
        _userRepository = userRepository;
        _userFactory = userFactory;
    }
    
    public async Task<string> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetByEmailAsync(command.Email);
        if (existingUser is not null)
            throw new InvalidOperationException($"A user with the email '{command.Email}' already exists.");
        var existingUsername = await _userRepository.GetByUsernameAsync(command.Username);
        if (existingUsername is not null)
            throw new InvalidOperationException($"A user with the username '{command.Username}' already exists.");
        var user = _userFactory.Create(command.Username, command.Firstname, command.Lastname, command.Email, command.Password);
        var email = EmailOutboxMessage.Verification(user.Email, user.FirstName, user.EmailVerificationDeliveryToken);
        var userId = await _userRepository.AddWithEmailAsync(user, email, cancellationToken);
        return userId;
    }
}

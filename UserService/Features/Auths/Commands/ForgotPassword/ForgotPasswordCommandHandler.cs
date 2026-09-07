using JobTracker.UserService.Application.Repositories;
using JobTracker.UserService.Application.Services;
using MediatR;

namespace JobTracker.UserService.Application.Auths.Commands.ForgotPassword;

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, bool>
{
    private readonly IUserRepository _userRepository;

    public ForgotPasswordCommandHandler(
        IUserRepository userRepository
    )
    {
        _userRepository = userRepository;
    }
    
    public async Task<bool> Handle(ForgotPasswordCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(command.Email);
        // Do not reveal whether an account exists.
        if (user is null)
            return true;
        var otp = user.ForgotPassword();
        var email = EmailOutboxMessage.PasswordReset(user.Email, user.FirstName, otp);
        await _userRepository.UpdateWithEmailAsync(user, email, cancellationToken);
        return true;
    }
}

using JobTracker.UserService.Application.Repositories;
using MediatR;

namespace JobTracker.UserService.Application.Auths.Commands.ResetPassword;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, bool>
{
    private readonly IUserRepository _userRepository;

    public ResetPasswordCommandHandler(
        IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<bool> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(command.Email);
        if (user is null)
            throw new InvalidOperationException("The password reset code is invalid or has expired.");
        if (!user.TryConsumePasswordResetOtp(command.OTP))
        {
            await _userRepository.UpdateAsync(user);
            throw new InvalidOperationException("The password reset code is invalid or has expired.");
        }
        user.ResetPassword(command.Password);
        await _userRepository.UpdateAsync(user);
        return true;
    }
}

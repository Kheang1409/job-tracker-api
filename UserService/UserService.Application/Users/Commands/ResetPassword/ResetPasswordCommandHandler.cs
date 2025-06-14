using JobTracker.UserService.Application.Repositories;
using MediatR;

namespace JobTracker.UserService.Application.Users.Commands.ResetPassword;

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
        var existingUser = await _userRepository.GetByOTPAsync(command.OTP);
        if(existingUser is null)
            throw new InvalidOperationException($"The provided OTP '{command.OTP}' is invalid or has expired.");
        existingUser.ResetPassword(command.Password);
        await _userRepository.UpdateAsync(existingUser);
        return true;
    }
}
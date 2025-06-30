using JobTracker.UserService.Application.Repositories;
using MediatR;
using MongoDB.Bson;

namespace JobTracker.UserService.Application.Auths.Commands.ResetPassword;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordWithIdCommand, bool>
{
    private readonly IUserRepository _userRepository;

    public ResetPasswordCommandHandler(
        IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<bool> Handle(ResetPasswordWithIdCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(command.userId);
        if (user is null || string.IsNullOrEmpty(user?.OTP))
            throw new InvalidOperationException($"The provided OTP '{command.OTP}' is invalid or has expired.");
        user.ResetPassword(command.Password);
        await _userRepository.UpdateAsync(user);
        return true;
    }
}
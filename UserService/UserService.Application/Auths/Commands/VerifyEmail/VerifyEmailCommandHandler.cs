using JobTracker.UserService.Application.Repositories;
using MediatR;

namespace JobTracker.UserService.Application.Auths.Commands.VerifyEmail;

public sealed class VerifyEmailCommandHandler(IUserRepository userRepository)
    : IRequestHandler<VerifyEmailCommand, bool>
{
    public async Task<bool> Handle(VerifyEmailCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(command.Email)
            ?? throw new InvalidOperationException("The verification request is invalid.");
        user.VerifyEmail(command.Token);
        await userRepository.UpdateAsync(user);
        return true;
    }
}

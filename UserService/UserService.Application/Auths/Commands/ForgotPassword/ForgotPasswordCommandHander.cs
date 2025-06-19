using JobTracker.SharedKernel.Messaging;
using JobTracker.UserService.Application.Repositories;
using MediatR;

namespace JobTracker.UserService.Application.Auths.Commands.ForgotPassword;

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IKafkaProducer _kafkaProducer;

    public ForgotPasswordCommandHandler(
        IUserRepository userRepository,
        IKafkaProducer kafkaProducer
    )
    {
        _userRepository = userRepository;
        _kafkaProducer = kafkaProducer;
    }
    
    public async Task<bool> Handle(ForgotPasswordCommand command, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetByEmailAsync(command.Email);
        if(existingUser is null)
            throw new InvalidOperationException($"A user with the email '{command.Email}' no exists.");
        existingUser.ForgotPassword();
        var notificationPayload = new
            {
                Type = "Auth",
                existingUser.FirstName,
                existingUser.Email,
                existingUser.OTP
            };
        await Task.WhenAll(
            _userRepository.UpdateAsync(existingUser),
            _kafkaProducer.Produce("job-tracker-topic", Guid.NewGuid().ToString(), notificationPayload)
        );
        return true;
    }
}
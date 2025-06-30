using JobTracker.SharedKernel.Messaging;
using JobTracker.UserService.Application.Repositories;
using JobTracker.UserService.Application.Services;
using MediatR;

namespace JobTracker.UserService.Application.Auths.Commands.ForgotPassword;

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, string>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    private readonly IKafkaProducer _kafkaProducer;

    public ForgotPasswordCommandHandler(
        IUserRepository userRepository,
        IJwtService jwtService,
        IKafkaProducer kafkaProducer
    )
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _kafkaProducer = kafkaProducer;
    }
    
    public async Task<string> Handle(ForgotPasswordCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(command.Email);
        if(user is null)
            throw new InvalidOperationException($"A user with the email '{command.Email}' no exists.");
        user.ForgotPassword();
        var notificationPayload = new
            {
                Type = "Auth",
                user.FirstName,
                user.Email,
                user.OTP
            };
        await Task.WhenAll(
            _userRepository.UpdateAsync(user),
            _kafkaProducer.Produce("job-tracker-topic", Guid.NewGuid().ToString(), notificationPayload)
        );
        var token = await _jwtService.GenerateToken(user);
        return token;
    }
}
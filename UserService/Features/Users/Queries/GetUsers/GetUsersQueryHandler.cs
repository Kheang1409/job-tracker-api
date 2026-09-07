using JobTracker.UserService.Application.Repositories;
using JobTracker.UserService.Domain.Entities;
using MediatR;

namespace JobTracker.UserService.Application.Users.Queries.GetUsers;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<User>>
{
    private readonly IUserRepository _userRepository;

    public GetUsersQueryHandler(
        IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<IEnumerable<User>> Handle(GetUsersQuery command, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllAsync(
            command.Fullname ?? string.Empty,
            command.Skill ?? string.Empty,
            command.PageNumber,
            command.Limit);
        return users;
    }
}
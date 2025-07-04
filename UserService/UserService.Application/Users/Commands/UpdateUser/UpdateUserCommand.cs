using JobTracker.UserService.Application.Commons;
using MediatR;

namespace JobTracker.UserService.Application.Users.Commands.UpdateUser;

public record UpdateUserCommand(
    string FirstName,
    string LastName,
    string Bio,
    string Gender,
    string Email,
    string CountryCode,
    string PhoneNumber,
    string Country,
    string Street,
    string City,
    string State,
    int PostalCode,
    List<CommandSkill> Skills
) : IRequest<bool>;
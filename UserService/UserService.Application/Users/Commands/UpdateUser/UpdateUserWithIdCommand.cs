using JobTracker.UserService.Domain.Enum;
using MediatR;

namespace JobTracker.UserService.Application.Users.Commands.UpdateUser;

public record UpdateUserWithIdCommand(string Id, string Firstname, string Lastname, string Bio, Gender Gender, string Email, string CountryCode, string PhoneNumber) : IRequest<string>;
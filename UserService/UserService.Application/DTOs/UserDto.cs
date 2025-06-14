using JobTracker.UserService.Domain.Entities;

namespace JobTracker.UserService.Application.DTOs;

public class UserDto
{
    public string Id { get; private set; } = string.Empty;
    public string Firstname { get; private set; } = string.Empty;
    public string Lastname { get; private set; } = string.Empty;
    public string Bio { get; private set; } = string.Empty;

    public static explicit operator UserDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Firstname = user.Firstname,
            Lastname = user.Lastname,
            Bio = user.Bio
        };
    }
}
using JobTracker.UserService.Domain.Entities;

namespace JobTracker.UserService.Application.DTOs;

public class UserDetailDto
{
    public string Id { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Bio { get; private set; } = string.Empty;
    public string Gender { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;
    public Address? Address { get; private set; }
    public IEnumerable<Skill>? Skills { get; private set; }
    public IEnumerable<Experience>? Experiences { get; private set; }
    public IEnumerable<Project>? Projects { get; private set; }

    public static explicit operator UserDetailDto(User user)
    {
        return new UserDetailDto
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Bio = user.Bio,
            Gender = user.Gender.ToString(),
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Skills = user.Skills,
            Address = user.Address,
            Experiences = user.Experiences,
            Projects= user.Projects
        };
    }
}
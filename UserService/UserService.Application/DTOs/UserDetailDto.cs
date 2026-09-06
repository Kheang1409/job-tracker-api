using JobTracker.UserService.Domain.Entities;
using JobTracker.SharedKernel.Domain;

namespace JobTracker.UserService.Application.DTOs;

public class UserDetailDto
{
    public string Id { get; private set; } = string.Empty;
    public string Username { get; private set; } = string.Empty;
    public bool IsEmailVerified { get; private set; }
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Bio { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string ContactNumber { get; private set; } = string.Empty;
    public string ContactCountry { get; private set; } = string.Empty;
    public Address? Address { get; private set; }
    public IEnumerable<string>? Skills { get; private set; }
    public IEnumerable<Experience>? Experiences { get; private set; }
    public IEnumerable<Project>? Projects { get; private set; }

    public static explicit operator UserDetailDto(User user)
    {
        return new UserDetailDto
        {
            Id = user.Id,
            Username = user.Username,
            IsEmailVerified = user.IsEmailVerified,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Bio = user.Bio,
            Email = user.Email,
            ContactNumber = user.ContactNumber,
            ContactCountry = user.ContactCountry,
            Skills = user.Skills,
            Address = user.Address,
            Experiences = user.Experiences,
            Projects = user.Projects
        };
    }
}
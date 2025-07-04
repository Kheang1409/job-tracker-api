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
    public ContactNumber? ContactNumber { get; private set; }
    public Address? Address { get; private set; }
    public IEnumerable<SkillDto>? Skills { get; private set; }
    public IEnumerable<Experience>? Experiences { get; private set; }
    public IEnumerable<Project>? Projects { get; private set; }

    public static explicit operator UserDetailDto(User user)
    {
        return new UserDetailDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Bio = user.Bio,
            Gender = user.Gender.ToString(),
            Email = user.Email,
            ContactNumber = user.ContactNumber,
            Skills = user.Skills.Select(skill => (SkillDto)skill),
            Address = user.Address,
            Experiences = user.Experiences,
            Projects = user.Projects
        };
    }
    
    public class SkillDto
    {
        public string Name { get; private set; } = string.Empty;

        public static explicit operator SkillDto(Skill skill)
        {
            return new SkillDto
            {
                Name = skill.Name
            };
        }
    }
}
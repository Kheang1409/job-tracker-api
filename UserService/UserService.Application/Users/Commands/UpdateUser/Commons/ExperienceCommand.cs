namespace JobTracker.UserService.Application.Users.Commands.UpdateUser.Commons;

public record ExperienceCommand(
    string CompanyName,
    string Position,
    List<string> BulletPoints,
    DateTime StartDate,
    DateTime? EndDate,
    bool IsPresent
);
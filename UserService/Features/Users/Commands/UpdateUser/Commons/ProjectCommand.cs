namespace JobTracker.UserService.Application.Users.Commands.UpdateUser.Commons;

public record ProjectCommand(
    string Name,
    string About,
    DateTime During,
    string Link
);
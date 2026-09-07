namespace JobTracker.UserService.Application.Users.Commands.UpdateUser.Commons;

public record AddressCommand(
    string Country,
    string State,
    string City,
    string Street,
    int PostalCode
);

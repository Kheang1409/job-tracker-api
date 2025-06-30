using JobTracker.UserService.Domain.Enums;

namespace JobTracker.UserService.Domain.Commons;

public class EnumParser
{
    public static Gender Gender(string input)
    {
        try
        {
            return (Gender)Enum.Parse(typeof(Gender), input, true);
        }
        catch (ArgumentException)
        {
            throw new InvalidOperationException("Invalid gender code");
        }
    }

    public static UserRole UserRole(string input)
    {
        try
        {
            return (UserRole)Enum.Parse(typeof(UserRole), input, true);
        }
        catch (ArgumentException)
        {
            throw new InvalidOperationException("Invalid user role code");
        }
    }
}
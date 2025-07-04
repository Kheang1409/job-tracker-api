namespace JobTracker.UserService.Domain.Entities;

public class ContactNumber
{
    public string CountryCode { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;

    private ContactNumber(string countryCode, string phoneNumber)
    {
        CountryCode = countryCode;
        PhoneNumber = phoneNumber;
    }
    public static ContactNumber Create(string countryCode, string phoneNumber)
    {
        return new ContactNumber(countryCode, phoneNumber);
    }
}
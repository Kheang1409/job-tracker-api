using System.Security.Cryptography;
using System.Text;
using JobTracker.UserService.Domain.Enum;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JobTracker.UserService.Domain.Entities;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; private set; } = string.Empty;
    public string Firstname { get; private set; } = string.Empty;
    public string Lastname { get; private set; } = string.Empty;
    public string Bio { get; private set; } = string.Empty;
    public Gender Gender { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string CountryCode { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string OTP { get; private set; } = string.Empty;
    public DateTime ExpireDate { get; private set; }
    public UserRole Role { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime ModifiedAt { get; private set; }
    public List<Skill> Skills { get; private set; } = new();
    public List<Experience> Experiences { get; private set; } = new();
    public List<Project> Projects { get; private set; } = new();
    public List<Address> Addresses { get; private set; } = new();

    public User() { }
    private User(string email, string password, UserRole role)
    {
        Email = email;
        PasswordHash = Hash(password);
        Role = role;
        CreatedAt = DateTime.UtcNow;
    }

    public static User Create(string email, string password, UserRole role)
    {
        var user = new User(email, password, role);
        return user;
    }

    public void Update(string firstname, string lastname, string bio, Gender gender, string email, string countryCode, string phoneNumber)
    {
        Email = email;
        Firstname = firstname;
        Lastname = lastname;
        Bio = bio;
        Gender = gender;
        Email = email;
        CountryCode = countryCode;
        PhoneNumber = phoneNumber;
        ModifiedAt = DateTime.UtcNow;
    }

    public static string NormalizePhoneNumber(string phone)
    {
        return new string(phone.Where(char.IsDigit).ToArray());
    }

    private string Hash(string password)
    {
        using var sha256 = SHA256.Create();
        return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
    }

    public void ForgotPassword()
    {
        ExpireDate = DateTime.UtcNow.AddMinutes(3);
        GenerateOtp();
    }

    public void ResetPassword(string password)
    {
        PasswordHash = Hash(password);
    }

    private void GenerateOtp()
    {
        var random = new Random();
        OTP = random.Next(100000, 999999).ToString();
    }

    public bool Verify(string plainPassword)
    {
        var computed = Hash(plainPassword);
        return computed == PasswordHash;
    }
}

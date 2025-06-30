using System.Security.Cryptography;
using System.Text;
using JobTracker.UserService.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JobTracker.UserService.Domain.Entities;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Bio { get; private set; } = string.Empty;
    public Gender Gender { get; private set; }
    public string Email { get; private set; } = string.Empty;
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
    public Address? Address { get; private set; }

    public User() { }
    private User(string firstName, string lastName, string email, string password, UserRole role)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PasswordHash = Hash(password);
        Role = role;
        CreatedAt = DateTime.UtcNow;
    }

    public static User Create(string firstName, string lastName, string email, string password, UserRole role)
    {
        var user = new User(firstName, lastName, email, password, role);
        return user;
    }

    public void SetAddress(Address address)
    {
        Address = address ?? throw new ArgumentNullException(nameof(address), "Address cannot be null");
    }

    public void Update(string firstName, string lastName, string bio, Gender gender, string email, string phoneNumber)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        Bio = bio;
        Gender = gender;
        Email = email;
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
        OTP = string.Empty;
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

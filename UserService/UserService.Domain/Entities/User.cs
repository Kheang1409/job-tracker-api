using System.Security.Cryptography;
using System.Text;
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
    public string Email { get; private set; } = string.Empty;
    public string ContactNumber { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string OTP { get; private set; } = string.Empty;
    public DateTime? ExpireDate { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ModifiedAt { get; private set; }
    public List<string> Skills { get; private set; } = new();
    public List<Experience> Experiences { get; private set; } = new();
    public List<Project> Projects { get; private set; } = new();
    public Address? Address { get; private set; }

    private User(Builder builder)
    {
        FirstName = builder.FirstName;
        LastName = builder.LastName;
        Email = builder.Email;
        ContactNumber = builder.ContactNumber;
        PasswordHash = Hash(builder.Password);
        Bio = builder.Bio;
        Skills = builder.Skills ?? new List<string>();
        Experiences = builder.Experiences ?? new List<Experience>();
        Projects = builder.Projects ?? new List<Project>();
        Address = builder.Address;
        CreatedAt = DateTime.UtcNow;
        GenerateOtp();
    }

    public class Builder
    {
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string Password { get; private set; } = string.Empty;
        public string Bio { get; private set; } = string.Empty;
        public string OTP { get; private set; } = string.Empty;
        public List<string> Skills { get; private set; } = new();
        public List<Experience> Experiences { get; private set; } = new();
        public List<Project> Projects { get; private set; } = new();
        public Address? Address { get; private set; }
        public string ContactNumber { get; private set; } = string.Empty;

        public Builder SetFirstName(string firstName)
        {
            FirstName = firstName;
            return this;
        }

        public Builder SetLastName(string lastName)
        {
            LastName = lastName;
            return this;
        }

        public Builder SetEmail(string email)
        {
            Email = email;
            return this;
        }

        public Builder SetContactNumber(string contactNumber)
        {
            ContactNumber = contactNumber;
            return this;
        }

        public Builder SetPassword(string password)
        {
            Password = password;
            return this;
        }

        public Builder SetBio(string bio)
        {
            Bio = bio;
            return this;
        }

        public Builder SetSkills(List<string> skills)
        {
            Skills = skills;
            return this;
        }

        public Builder SetExperiences(List<Experience> experiences)
        {
            Experiences = experiences;
            return this;
        }

        public Builder SetProjects(List<Project> projects)
        {
            Projects = projects;
            return this;
        }

        public Builder SetAddress(Address? address)
        {
            Address = address;
            return this;
        }

        public User Build()
        {
            if (string.IsNullOrEmpty(FirstName))
                throw new ArgumentNullException(nameof(FirstName), "FirstName cannot be null or empty.");
            if (string.IsNullOrEmpty(LastName))
                throw new ArgumentNullException(nameof(LastName), "LastName cannot be null or empty.");
            if (string.IsNullOrEmpty(Email))
                throw new ArgumentNullException(nameof(Email), "Email cannot be null or empty.");
            if (string.IsNullOrEmpty(Password))
                throw new ArgumentNullException(nameof(Password), "Password cannot be null or empty.");

            return new User(this);
        }
    }

    public void UpdateProfile(
        string firstName,
        string lastName,
        string email,
        string contactNumber,
        string bio,
        List<string> skills,
        List<Experience> experiences,
        List<Project> projects,
        Address address)
    {
        FirstName = firstName;
        LastName = lastName;
        ContactNumber = contactNumber;
        Email = email;
        Bio = bio;
        Skills = skills;
        Experiences = experiences;
        Projects = projects;
        Address = address;
        ModifiedAt = DateTime.UtcNow;
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

    public bool Verify(string plainPassword)
    {
        var computed = Hash(plainPassword);
        return computed == PasswordHash;
    }
    
    private void GenerateOtp()
    {
        var random = new Random();
        OTP = random.Next(100000, 999999).ToString();
    }
}

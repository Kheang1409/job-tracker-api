using System.Security.Cryptography;
using System.Text;
using JobTracker.SharedKernel.Domain;

namespace JobTracker.UserService.Domain.Entities;

public class User : AggregateRoot
{
    public string Id { get; private set; } = string.Empty;
    public string Username { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Bio { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string ContactNumber { get; private set; } = string.Empty;
    public string ContactCountry { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public bool IsEmailVerified { get; private set; }
    public string EmailVerificationToken { get; private set; } = string.Empty;
    public string EmailVerificationDeliveryToken { get; private set; } = string.Empty;
    public DateTime? EmailVerificationExpiresAt { get; private set; }
    public string OTP { get; private set; } = string.Empty;
    public DateTime? ExpireDate { get; private set; }
    public int PasswordResetFailedAttempts { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ModifiedAt { get; private set; }
    private List<string> _skills = [];
    private List<Experience> _experiences = [];
    private List<Project> _projects = [];

    public IReadOnlyList<string> Skills => _skills.AsReadOnly();
    public IReadOnlyList<Experience> Experiences => _experiences.AsReadOnly();
    public IReadOnlyList<Project> Projects => _projects.AsReadOnly();
    public Address? Address { get; private set; }

    private User(Builder builder)
    {
        Id = NewObjectId();
        Username = builder.Username.Trim();
        FirstName = builder.FirstName.Trim();
        LastName = builder.LastName.Trim();
        Email = NormalizeEmail(builder.Email);
        ContactNumber = builder.ContactNumber;
        PasswordHash = HashPassword(builder.Password);
        Bio = builder.Bio;
        ReplaceSkills(builder.Skills);
        ReplaceExperiences(builder.Experiences);
        ReplaceProjects(builder.Projects);
        Address = builder.Address;
        CreatedAt = DateTime.UtcNow;
        GenerateEmailVerificationToken();
    }

    public class Builder
    {
        public string FirstName { get; private set; } = string.Empty;
        public string Username { get; private set; } = string.Empty;
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

        public Builder SetUsername(string username)
        {
            Username = username;
            return this;
        }

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
            if (string.IsNullOrWhiteSpace(FirstName))
                throw new ArgumentNullException(nameof(FirstName), "FirstName cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(LastName))
                throw new ArgumentNullException(nameof(LastName), "LastName cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(Email))
                throw new ArgumentNullException(nameof(Email), "Email cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(Username))
                throw new ArgumentNullException(nameof(Username), "Username cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(Password))
                throw new ArgumentNullException(nameof(Password), "Password cannot be null or empty.");

            var user = new User(this);
            return user;
        }
    }

    public void UpdateProfile(
        string firstName,
        string lastName,
        string email,
        string contactNumber,
        string contactCountry,
        string bio,
        List<string> skills,
        List<Experience> experiences,
        List<Project> projects,
        Address address)
    {
        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("First and last name are required.");
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.", nameof(email));
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        ContactNumber = contactNumber?.Trim() ?? string.Empty;
        ContactCountry = contactCountry?.Trim() ?? string.Empty;
        Email = NormalizeEmail(email);
        Bio = bio?.Trim() ?? string.Empty;
        ReplaceSkills(skills);
        ReplaceExperiences(experiences);
        ReplaceProjects(projects);
        Address = address ?? throw new ArgumentNullException(nameof(address));
        ModifiedAt = DateTime.UtcNow;
    }

    public void AddSkill(string skill)
    {
        var normalized = NormalizeRequired(skill, nameof(skill));
        if (!_skills.Contains(normalized, StringComparer.OrdinalIgnoreCase))
            _skills.Add(normalized);
    }

    public bool RemoveSkill(string skill) =>
        _skills.RemoveAll(existing =>
            string.Equals(existing, skill?.Trim(), StringComparison.OrdinalIgnoreCase)) > 0;

    public void AddExperience(Experience experience) =>
        _experiences.Add(experience ?? throw new ArgumentNullException(nameof(experience)));

    public bool RemoveExperience(Experience experience) => _experiences.Remove(experience);

    public void AddProject(Project project) =>
        _projects.Add(project ?? throw new ArgumentNullException(nameof(project)));

    public bool RemoveProject(Project project) => _projects.Remove(project);

    private void ReplaceSkills(IEnumerable<string>? skills)
    {
        _skills.Clear();
        foreach (var skill in skills ?? [])
            AddSkill(skill);
    }

    private void ReplaceExperiences(IEnumerable<Experience>? experiences)
    {
        _experiences.Clear();
        foreach (var experience in experiences ?? [])
            AddExperience(experience);
    }

    private void ReplaceProjects(IEnumerable<Project>? projects)
    {
        _projects.Clear();
        foreach (var project in projects ?? [])
            AddProject(project);
    }

    private static string NormalizeRequired(string value, string parameterName)
    {
        var normalized = value?.Trim();
        return string.IsNullOrWhiteSpace(normalized)
            ? throw new ArgumentException("Value cannot be empty.", parameterName)
            : normalized;
    }

    private static string HashPassword(string password)
    {
        const int iterations = 120_000;
        var salt = RandomNumberGenerator.GetBytes(16);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            iterations,
            HashAlgorithmName.SHA256,
            32);
        return $"PBKDF2-SHA256${iterations}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
    }

    public string ForgotPassword()
    {
        ExpireDate = DateTime.UtcNow.AddMinutes(3);
        PasswordResetFailedAttempts = 0;
        var otp = RandomNumberGenerator.GetInt32(100000, 1_000_000).ToString();
        OTP = HashSecurityToken(otp);
        return otp;
    }

    public void VerifyEmail(string token)
    {
        if (IsEmailVerified ||
            string.IsNullOrWhiteSpace(EmailVerificationToken) ||
            !VerifySecurityToken(EmailVerificationToken, token) ||
            EmailVerificationExpiresAt is null ||
            EmailVerificationExpiresAt <= DateTime.UtcNow)
            throw new InvalidOperationException("The email verification token is invalid or has expired.");

        IsEmailVerified = true;
        EmailVerificationToken = string.Empty;
        EmailVerificationExpiresAt = null;
    }

    public void ResetPassword(string password)
    {
        OTP = string.Empty;
        ExpireDate = null;
        PasswordResetFailedAttempts = 0;
        PasswordHash = HashPassword(password);
    }

    public bool Verify(string plainPassword)
    {
        if (PasswordHash.StartsWith("PBKDF2-SHA256$", StringComparison.Ordinal))
        {
            var parts = PasswordHash.Split('$');
            if (parts.Length != 4 || !int.TryParse(parts[1], out var iterations))
                return false;

            try
            {
                var salt = Convert.FromBase64String(parts[2]);
                var expected = Convert.FromBase64String(parts[3]);
                var actual = Rfc2898DeriveBytes.Pbkdf2(
                    plainPassword, salt, iterations, HashAlgorithmName.SHA256, expected.Length);
                return CryptographicOperations.FixedTimeEquals(actual, expected);
            }
            catch (FormatException)
            {
                return false;
            }
        }

        // Legacy SHA-256 hashes are accepted once so existing users can migrate on login.
        using var sha256 = SHA256.Create();
        var legacy = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(plainPassword)));
        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(legacy),
            Encoding.UTF8.GetBytes(PasswordHash));
    }

    public bool NeedsPasswordRehash =>
        !PasswordHash.StartsWith("PBKDF2-SHA256$", StringComparison.Ordinal);

    public void RehashPassword(string password) => PasswordHash = HashPassword(password);
    
    public bool TryConsumePasswordResetOtp(string otp)
    {
        if (string.IsNullOrWhiteSpace(OTP) || ExpireDate is null ||
            ExpireDate <= DateTime.UtcNow || PasswordResetFailedAttempts >= 5)
            return false;

        if (VerifySecurityToken(OTP, otp))
            return true;

        PasswordResetFailedAttempts++;
        if (PasswordResetFailedAttempts >= 5)
        {
            OTP = string.Empty;
            ExpireDate = null;
        }
        return false;
    }

    private void GenerateEmailVerificationToken()
    {
        EmailVerificationDeliveryToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
        EmailVerificationToken = HashSecurityToken(EmailVerificationDeliveryToken);
        EmailVerificationExpiresAt = DateTime.UtcNow.AddHours(24);
    }

    private static string HashSecurityToken(string token) =>
        $"SHA256${Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)))}";

    private static bool VerifySecurityToken(string storedToken, string suppliedToken)
    {
        var expected = storedToken.StartsWith("SHA256$", StringComparison.Ordinal)
            ? storedToken
            : HashSecurityToken(storedToken);
        var actual = HashSecurityToken(suppliedToken);
        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(expected), Encoding.UTF8.GetBytes(actual));
    }

    private static string NormalizeEmail(string email) => email.Trim().ToLowerInvariant();

    private static string NewObjectId() =>
        Convert.ToHexString(RandomNumberGenerator.GetBytes(12)).ToLowerInvariant();
}

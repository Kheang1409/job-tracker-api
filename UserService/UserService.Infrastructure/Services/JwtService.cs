using JobTracker.UserService.Application.Services;
using JobTracker.UserService.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JobTracker.UserService.Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<string> GenerateToken(User user)
        {
            // Validate user properties
            if (string.IsNullOrEmpty(user.Id))
                throw new ArgumentNullException(nameof(user.Id), "User ID cannot be null or empty.");
            if (string.IsNullOrEmpty(user.Email))
                throw new ArgumentNullException(nameof(user.Email), "Email cannot be null or empty.");
            if (string.IsNullOrEmpty(user.FirstName))
                throw new ArgumentNullException(nameof(user.FirstName), "First name cannot be null or empty.");
            if (string.IsNullOrEmpty(user.LastName))
                throw new ArgumentNullException(nameof(user.LastName), "Last name cannot be null or empty.");
            // Retrieve JWT settings
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY")
                            ?? jwtSettings["SecretKey"]
                            ?? throw new InvalidOperationException("JWT SecretKey is missing. Please provide it via environment or configuration.");;
            var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") 
                        ?? jwtSettings["Issuer"]
                        ?? throw new InvalidOperationException("JWT Issuer is missing. Please provide it via environment or configuration.");
            var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") 
                        ?? jwtSettings["Audience"]
                        ?? throw new InvalidOperationException("JWT Audience is missing. Please provide it via environment or configuration.");
            var expiryMinutesString = Environment.GetEnvironmentVariable("JWT_EXPIRY_MINUTES") 
                        ?? jwtSettings["ExpiryMinutes"]
                        ?? throw new InvalidOperationException("ExpiryMinutes is missing. Please provide it via environment or configuration.");

            // Validate that configuration settings are available
            if (string.IsNullOrEmpty(secretKey))
                throw new ArgumentNullException("SecretKey", "SecretKey cannot be null or empty.");
            if (string.IsNullOrEmpty(issuer))
                throw new ArgumentNullException("Issuer", "Issuer cannot be null or empty.");
            if (string.IsNullOrEmpty(audience))
                throw new ArgumentNullException("Audience", "Audience cannot be null or empty.");
            if (string.IsNullOrEmpty(expiryMinutesString))
                throw new ArgumentNullException("ExpiryMinutes", "ExpiryMinutes cannot be null or empty.");

            // Parse the expiry minutes (with error handling)
            if (!int.TryParse(expiryMinutesString, out var expiryMinutes))
                throw new ArgumentException("ExpiryMinutes must be a valid integer.");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Prepare claims
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // JWT ID (unique identifier)
                new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName)
            };

            // Create JWT token
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: credentials
            );

            // Generate the token string
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return Task.FromResult(tokenString);
        }
    }
}
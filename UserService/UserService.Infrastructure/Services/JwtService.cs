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
            
            // Retrieve JWT settings
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var expiryMinutesString = jwtSettings["ExpiryMinutes"];

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
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // JWT ID (unique identifier)
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
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace JobTracker.UserService.Infrastructure.Extensions;

public static class AuthenticationServiceCollectionExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") 
                        ?? jwtSettings["SecretKey"]
                        ?? throw new InvalidOperationException("JWT SecretKey is missing. Please provide it via environment or configuration.");
        var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") 
                        ?? jwtSettings["Issuer"]
                        ?? throw new InvalidOperationException("JWT Issuer is missing. Please provide it via environment or configuration.");
        var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") 
                        ?? jwtSettings["Audience"]
                        ?? throw new InvalidOperationException("JWT Audience is missing. Please provide it via environment or configuration.");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ClockSkew = TimeSpan.FromMinutes(5)
            };
        });

        return services;
    }
}
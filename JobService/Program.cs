using System.Text;
using JobService.AutoMapper;
using JobService.Kafka;
using JobService.Repositories;
using JobService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token in the format: `Bearer {your token}`",
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
}); // Add Swagger

// MongoDB Configuration - Prefer environment variables over appsettings.json
string mongoConnectionString = Environment.GetEnvironmentVariable("MongoDB__ConnectionString")
                               ?? builder.Configuration.GetConnectionString("MongoDB");


builder.Services.AddSingleton<IKafkaProducer, KafkaProducer>();

// MongoDB Configuration
builder.Services.AddSingleton<IMongoClient>(_ => new MongoClient(mongoConnectionString));

// Repository registration
builder.Services.AddScoped<IJobRepository, JobRepository>();
builder.Services.AddScoped<IJobService, JobService.Services.JobService>();

// Add AutoMapper to the service container
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAutoMapper(typeof(JobProfile));

// Enable Authentication (JWT Bearer)
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine($"Token validated for: {context.Principal?.Identity?.Name}");
                return Task.CompletedTask;
            }
        };
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],

            ValidateAudience = true,
            ValidAudiences = new[] { builder.Configuration["JwtSettings:Audience"] }, // Updated

            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"])
            )
        };

    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Enable Swagger in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware order is crucial
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

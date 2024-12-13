using UserService.Repositories;
using MongoDB.Driver;
using UserService.Services;
using UserService.Kafka;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IKafkaProducer, KafkaProducer>();

// MongoDB Configuration - Prefer environment variables over appsettings.json
string mongoConnectionString = Environment.GetEnvironmentVariable("MongoDB__ConnectionString")
                               ?? builder.Configuration.GetConnectionString("MongoDB");

// MongoDB Configuration
builder.Services.AddSingleton<IMongoClient>(_ => new MongoClient(mongoConnectionString));

// Register UserRepository
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Register JwtService
builder.Services.AddScoped<IJwtService, JwtService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

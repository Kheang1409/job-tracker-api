using Confluent.Kafka;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using NotificationService.Filters;
using NotificationService.KafkaConsumers;
using NotificationService.Models;

var builder = WebApplication.CreateBuilder(args);

// EmailSettings registration
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection(nameof(EmailSettings)));

// Bind Kafka Configuration
var kafkaConfig = builder.Configuration.GetSection("Kafka");
var bootstrapServers = kafkaConfig.GetValue<string>("BootstrapServers");

builder.Services.AddControllers();
builder.Services.AddSingleton<INotificationService, NotificationService.Services.NotificationService>();

// Register Kafka Consumer with dynamic configuration
builder.Services.AddSingleton<IConsumer<string, string>>(sp =>
{
    var config = new ConsumerConfig
    {
        BootstrapServers = bootstrapServers,
        GroupId = "notification-group",
        AutoOffsetReset = AutoOffsetReset.Earliest
    };

    return new ConsumerBuilder<string, string>(config).Build();
});

string mongoConnectionString = Environment.GetEnvironmentVariable("MongoDB__ConnectionString")
                               ?? builder.Configuration.GetConnectionString("MongoDB");

// Configure Hangfire with automatic migration
builder.Services.AddHangfire(config =>
{
    var storageOptions = new MongoStorageOptions
    {
        MigrationOptions = new MongoMigrationOptions
        {
            MigrationStrategy = new MigrateMongoMigrationStrategy(),
            BackupStrategy = new CollectionMongoBackupStrategy()
        }
    };

    config.UseMongoStorage(mongoConnectionString, "HangfireJobs", storageOptions);
});
builder.Services.AddHangfireServer();

// Register NotificationConsumer as a Singleton service
builder.Services.AddSingleton<NotificationConsumer>();
builder.Logging.AddConsole().SetMinimumLevel(LogLevel.Debug);

var app = builder.Build();

// Ensure Hangfire Dashboard is configured first
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new AllowAllDashboardAuthorizationFilter() }
});

// Add routing and controllers
app.UseRouting();
app.MapControllers();

// Run Hangfire jobs (e.g., recurring, background jobs)
BackgroundJob.Enqueue(() => Console.WriteLine("Hangfire jobs running"));

// Start the Kafka consumer service after Hangfire has finished its setup
app.Lifetime.ApplicationStarted.Register(() =>
{
    var notificationConsumer = app.Services.GetRequiredService<NotificationConsumer>();
    Task.Run(() => notificationConsumer.StartAsync(CancellationToken.None)); // Start Kafka consumer in the background
});

app.Run();
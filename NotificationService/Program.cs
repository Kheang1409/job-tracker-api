using Confluent.Kafka;
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

builder.Services.AddHostedService<NotificationConsumer>();

var app = builder.Build();

app.UseRouting();
app.MapControllers();
app.Run();

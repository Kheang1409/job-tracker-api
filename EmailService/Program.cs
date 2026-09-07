using JobTracker.EmailService;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<EmailSender>();
builder.Services.AddSingleton<EmailTemplateRenderer>();
builder.Services.AddHostedService<EmailConsumerWorker>();

await builder.Build().RunAsync();

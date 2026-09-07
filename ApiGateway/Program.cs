using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();
app.UseWebSockets();
app.Map("/health", health => health.Run(async context =>
{
    context.Response.ContentType = "application/json";
    await context.Response.WriteAsJsonAsync(new { Status = "Healthy" });
}));
await app.UseOcelot();
await app.RunAsync();

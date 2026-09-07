using JobTracker.SharedKernel.Middleware;
using JobTracker.SharedKernel.Extensions;
using JobTracker.UserService.Infrastructure.Persistence;
using JobTracker.UserService.API.Endpoints;
using JobTracker.UserService.API.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddApiRateLimiting();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddFrontendCors(builder.Configuration);

var app = builder.Build();
app.UseFrontendCors();
app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
if (app.Environment.IsDevelopment())
    app.UseHttpsRedirection();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.MapSharedHealthEndpoint();
app.MapAuthenticationEndpoints();
app.MapUserEndpoints();
app.Run();

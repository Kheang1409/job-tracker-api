using JobTracker.JobApplicationService.Endpoints;
using JobTracker.JobApplicationService.Hubs;
using JobTracker.JobApplicationService.Infrastructure.Persistence;
using JobTracker.SharedKernel.Extensions;
using JobTracker.SharedKernel.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddSignalR();
builder.Services.AddJobApplicationInfrastructure(builder.Configuration);
builder.Services.AddFrontendCors(builder.Configuration);

var app = builder.Build();
app.UseFrontendCors();
app.UseMiddleware<GlobalExceptionMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
}
app.UseAuthentication();
app.UseAuthorization();
app.MapSharedHealthEndpoint();
app.MapJobApplicationEndpoints();
app.MapHub<JobApplicationsHub>("/api/job-applications-hub");
app.Run();

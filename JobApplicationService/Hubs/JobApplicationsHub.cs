using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace JobTracker.JobApplicationService.Hubs;

[Authorize]
public sealed class JobApplicationsHub : Hub
{
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace JobTracker.UserService.API.Hubs;

[Authorize]
public sealed class JobApplicationsHub : Hub
{
}

using System.Security.Claims;

namespace JobTracker.JobApplicationService.Endpoints;

internal static class HttpContextUserExtensions
{
    internal static string GetUserId(this HttpContext context) =>
        context.User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new UnauthorizedAccessException();
}

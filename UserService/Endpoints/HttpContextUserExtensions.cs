using System.Security.Claims;

namespace JobTracker.UserService.API.Endpoints;

internal static class HttpContextUserExtensions
{
    internal static string GetUserId(this HttpContext context) =>
        context.User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new UnauthorizedAccessException();
}

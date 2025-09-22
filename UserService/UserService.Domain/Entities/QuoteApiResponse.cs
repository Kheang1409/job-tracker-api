namespace JobTracker.UserService.Domain.Entities;

public class QuoteApiResponse
{
    public string Quote { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
}
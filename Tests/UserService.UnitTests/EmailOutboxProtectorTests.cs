using JobTracker.UserService.Application.Services;
using JobTracker.UserService.Infrastructure.Persistence;
using JobTracker.UserService.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;

namespace JobTracker.UserService.UnitTests;

public sealed class EmailOutboxProtectorTests
{
    [Fact]
    public void Protect_EncryptsTokenAndMongoDocumentExcludesPlaintext()
    {
        MongoMappings.Register();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Outbox:EncryptionKey"] = "unit-test-outbox-key-at-least-32-characters"
            })
            .Build();
        var protector = new EmailOutboxProtector(configuration);
        var message = EmailOutboxMessage.PasswordReset("user@example.com", "Test", "123456");

        protector.Protect(message);
        var document = message.ToBsonDocument();

        Assert.Empty(message.Token);
        Assert.NotEmpty(message.EncryptedToken);
        Assert.False(document.Contains("Token"));
        Assert.DoesNotContain("123456", document.ToJson(), StringComparison.Ordinal);
        Assert.Equal("123456", protector.Unprotect(message));
    }
}

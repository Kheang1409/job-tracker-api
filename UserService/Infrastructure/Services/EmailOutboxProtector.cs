using System.Security.Cryptography;
using System.Text;
using JobTracker.UserService.Application.Services;
using Microsoft.Extensions.Configuration;

namespace JobTracker.UserService.Infrastructure.Services;

public sealed class EmailOutboxProtector
{
    private readonly byte[] _key;

    public EmailOutboxProtector(IConfiguration configuration)
    {
        var secret = Environment.GetEnvironmentVariable("OUTBOX_ENCRYPTION_KEY");
        if (string.IsNullOrWhiteSpace(secret))
            secret = configuration["Outbox:EncryptionKey"];
        if (string.IsNullOrWhiteSpace(secret))
            secret = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? configuration["JwtSettings:SecretKey"];
        if (string.IsNullOrWhiteSpace(secret))
            throw new InvalidOperationException("OUTBOX_ENCRYPTION_KEY or JWT_SECRET_KEY is required.");
        if (secret.Length < 32)
            throw new InvalidOperationException("The outbox encryption secret must be at least 32 characters.");
        _key = SHA256.HashData(Encoding.UTF8.GetBytes(secret));
    }

    public void Protect(EmailOutboxMessage message)
    {
        var plaintext = Encoding.UTF8.GetBytes(message.Token);
        var nonce = RandomNumberGenerator.GetBytes(12);
        var ciphertext = new byte[plaintext.Length];
        var tag = new byte[16];
        using var aes = new AesGcm(_key, tag.Length);
        aes.Encrypt(nonce, plaintext, ciphertext, tag, Encoding.UTF8.GetBytes(message.Id));
        message.EncryptedToken = Convert.ToBase64String(ciphertext);
        message.TokenNonce = Convert.ToBase64String(nonce);
        message.TokenTag = Convert.ToBase64String(tag);
        message.Token = string.Empty;
        CryptographicOperations.ZeroMemory(plaintext);
    }

    public string Unprotect(EmailOutboxMessage message)
    {
        var ciphertext = Convert.FromBase64String(message.EncryptedToken);
        var nonce = Convert.FromBase64String(message.TokenNonce);
        var tag = Convert.FromBase64String(message.TokenTag);
        var plaintext = new byte[ciphertext.Length];
        using var aes = new AesGcm(_key, tag.Length);
        aes.Decrypt(nonce, ciphertext, tag, plaintext, Encoding.UTF8.GetBytes(message.Id));
        try
        {
            return Encoding.UTF8.GetString(plaintext);
        }
        finally
        {
            CryptographicOperations.ZeroMemory(plaintext);
        }
    }
}

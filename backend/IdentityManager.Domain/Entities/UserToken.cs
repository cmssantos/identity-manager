using System.Security.Cryptography;
using IdentityManager.Domain.Exceptions;
using IdentityManager.Domain.Types;

namespace IdentityManager.Domain.Entities;

public class UserToken
{
    // Relações
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;

    // Propriedades
    public TokenType Type { get; private set; }
    public string Token { get; private set; }
    public DateTime ExpirationDate { get; private set; }
    public bool IsUsed { get; private set; }
    public bool IsRevoked { get; private set; }

    protected UserToken()
    {
        Token = string.Empty;
    }

    // Construtor
    public UserToken(User user, TokenType type, int expirationInMinutes = 360)
    {
        ArgumentNullException.ThrowIfNull(user);

        if (!Enum.IsDefined(typeof(TokenType), type))
        {
            throw new CustomArgumentException(ErrorCodes.InvalidUserTokenType);
        }

        if (expirationInMinutes <= 0)
        {
            throw new CustomArgumentException(ErrorCodes.InvalidUserTokenExpiration);
        }

        User = user;
        UserId = user.Id;
        Type = type;

        // Generate a random secure token
        Token = GenerateSecureRandomString();
        ExpirationDate = DateTime.UtcNow.AddMinutes(expirationInMinutes);
    }

    // Methods for manipulating the token
    public void MarkAsUsed() => IsUsed = true;

    public void Revoke() => IsRevoked = true;

    // Generate a random safe string
    private static string GenerateSecureRandomString(int sizeInBytes = 32)
    {
        byte[] buffer = new byte[sizeInBytes];
        RandomNumberGenerator.Fill(buffer);
        return Convert.ToBase64String(buffer);
    }
}

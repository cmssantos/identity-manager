using IdentityManager.Domain.Types;
using IdentityManager.Domain.ValueObjects;

namespace IdentityManager.Domain.Entities;

public class User
{
    // Properties
    public Guid Id { get; private set; }
    public string Username { get; private set; }
    public Email Email { get; private set; } = null!;
    public Password Password { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime? LastLogin { get; private set; }

    // Relations
    private readonly List<UserToken> _tokens = [];
    public IReadOnlyCollection<UserToken> Tokens => _tokens;

    protected User()
    {
        Id = Guid.NewGuid();
        Username = string.Empty;
    }

    public User(string username, Email email, Password password)
    {
        // Validates input arguments to ensure they are not null or empty
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException(ErrorCodes.UsernameCannotBeEmpty.ToString());
        }

        Id = Guid.NewGuid();

        Username = username;
        Email = email;
        Password = password;
        IsActive = false;

        CreatedAt = DateTime.Now;
        UpdatedAt = CreatedAt;
    }

    public void UpdateLastActivity() => UpdatedAt = DateTime.UtcNow;

    public void UpdateLastLogin()
    {
        LastLogin = DateTime.UtcNow;
        UpdateLastActivity();
    }

    public void Activate()
    {
        IsActive = true;
        UpdateLastActivity();
    }

    public void AddToken(UserToken newToken, bool revokeExistingToken = true)
    {
        // Checks for an existing token of the same type that has not been used or revoked
        var existingToken = Tokens
            .FirstOrDefault(token => token.Type == newToken.Type && !token.IsUsed && !token.IsRevoked);

        // If a token exists and revokeExistingToken is true, revoke the existing token
        if (existingToken != null)
        {
            if (revokeExistingToken)
            {
                existingToken.Revoke();
            }
            else
            {
                throw new ArgumentException(ErrorCodes.ExistingTokenFound.ToString());
            }
        }

        _tokens.Add(newToken);
    }
}

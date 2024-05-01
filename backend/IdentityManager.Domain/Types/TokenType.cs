namespace IdentityManager.Domain.Types;

public enum TokenType
{
    None = 0,
    Bearer = 1,
    Refresh = 2,
    PasswordReset = 3,
    AccountVerification = 4
}

namespace IdentityManager.Domain.Types;

public enum ErrorCodes
{
    // User errors
    UsernameCannotBeEmpty,
    UserTokenTypeNotValid,
    ExistingTokenFound,

    // UserToken errors
    InvalidUserTokenType,
    InvalidUserTokenExpiration,

    // Email errors
    NameCannotBeEmpty,
    EmailCannotBeEmpty,
    InvalidEmailFormat,

    // Password errors
    PasswordTooShort,
    PasswordCannotBeEmpty,
    PasswordMustContainDigit,
    PasswordMustContainUpperCase,
    PasswordMustContainLowerCase,
    PasswordMustContainSpecialCharacter,

    // Application errors
    UserAlreadyExists,
    UserNotFound
}

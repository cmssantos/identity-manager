using System.Security.Cryptography;
using System.Text;
using IdentityManager.Domain.Exceptions;
using IdentityManager.Domain.Types;

namespace IdentityManager.Domain.ValueObjects;

public sealed partial class Password : IEquatable<Password>
{
    private readonly string _hash;

    public string Value => _hash;

    // Constructor
    public Password(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new CustomArgumentException(ErrorCodes.PasswordCannotBeEmpty);
        }

        // Check if the password meets the desired complexity requirements
        if (password.Length < 8)
        {
            throw new CustomArgumentException(ErrorCodes.PasswordTooShort);
        }

        if (!HasUpperCase(password))
        {
            throw new CustomArgumentException(ErrorCodes.PasswordMustContainUpperCase);
        }

        if (!HasLowerCase(password))
        {
            throw new CustomArgumentException(ErrorCodes.PasswordMustContainLowerCase);
        }

        if (!HasDigit(password))
        {
            throw new CustomArgumentException(ErrorCodes.PasswordMustContainDigit);
        }

        if (!HasSpecialCharacter(password))
        {
            throw new CustomArgumentException(ErrorCodes.PasswordMustContainSpecialCharacter);
        }

        // Hash the plain password using a secure hash function
        _hash = HashPassword(password);
    }

    // Method to hash the plain password using a secure hash function
    private static string HashPassword(string plainPassword)
    {
        byte[] hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(plainPassword));
        return Convert.ToBase64String(hashBytes);
    }

    // Method to check whether the password contains capital letters
    private static bool HasUpperCase(string password) => password.Any(char.IsUpper);

    // Method to check if password contains lowercase letters
    private static bool HasLowerCase(string password) => password.Any(char.IsLower);

    // Method to check if password contains digits
    private static bool HasDigit(string password) => password.Any(char.IsDigit);

    // Method to check if the password contains special characters
    private static bool HasSpecialCharacter(string password) => !password.All(char.IsLetterOrDigit);

    // Method to validate a provided plain password against the stored hash
    public bool Validate(string plainPassword)
    {
        string hashToCompare = HashPassword(plainPassword);
        return _hash == hashToCompare;
    }

    // Equality methods
    public bool Equals(Password? other)
    {
        if (other == null)
        {
            return false;
        }
        return _hash == other._hash;
    }

    // Override Equals method to guarantee equality by value
    public override bool Equals(object? obj)
    {
        if (obj is Password otherPassword)
        {
            return Equals(otherPassword);
        }
        return false;
    }

    // Override GetHashCode method to be consistent with Equals
    public override int GetHashCode() => _hash.GetHashCode();

    // Override ToString to return the hash of the password
    public override string ToString() => _hash;
}

using System.Net.Mail;
using IdentityManager.Domain.Types;

namespace IdentityManager.Domain.ValueObjects;

public sealed partial class Email : IEquatable<Email>
{
    public string Value { get; }

    // Constructor
    public Email(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException(ErrorCodes.EmailCannotBeEmpty.ToString());
        }

        // Validate the email using MailAddress
        try
        {
            var mailAddress = new MailAddress(email);
            Value = mailAddress.Address;
        }
        catch (FormatException)
        {
            throw new ArgumentException(ErrorCodes.InvalidEmailFormat.ToString());
        }
    }

    // Method to check equality
    public bool Equals(Email? other)
    {
        if (other == null)
        {
            return false;
        }
        return Value == other.Value;
    }

    // Override the Equals method to guarantee equality by value
    public override bool Equals(object? obj)
    {
        if (obj is Email otherEmail)
        {
            return Equals(otherEmail);
        }
        return false;
    }

    // Override the GetHashCode method to be consistent with Equals
    public override int GetHashCode() => Value.GetHashCode();

    // Override ToString to return the email
    public override string ToString() => Value;
}

using IdentityManager.Domain.Types;

namespace IdentityManager.Domain.ValueObjects;

public sealed partial class EmailContact : IEquatable<EmailContact>
{
    public string Name { get; }
    public Email Email { get; }

    // Constructor
    public EmailContact(string name, Email email)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(ErrorCodes.NameCannotBeEmpty.ToString());
        }

        Name = name;
        Email = email ?? throw new ArgumentNullException(nameof(email));
    }

    public bool Equals(EmailContact? other)
    {
        if (other == null) return false;
        return Name == other.Name && Email.Equals(other.Email);
    }

    public override bool Equals(object? obj) => obj is EmailContact other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(Name, Email);

    public override string ToString() => $"{Name} <{Email}>";
}

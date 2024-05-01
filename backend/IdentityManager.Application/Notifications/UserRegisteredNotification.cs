using MediatR;

namespace IdentityManager.Application.Notifications;

public class UserRegisteredNotification : INotification
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string AccountVerificationToken { get; set; }

    public UserRegisteredNotification()
    {
        Username = string.Empty;
        Email = string.Empty;
        AccountVerificationToken = string.Empty;
    }

    public UserRegisteredNotification(string username, string email, string accountVerificationToken)
    {
        Username = username;
        Email = email;
        AccountVerificationToken = accountVerificationToken;
    }
}

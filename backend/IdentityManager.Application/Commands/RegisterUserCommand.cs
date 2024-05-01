using MediatR;

namespace IdentityManager.Application.Commands;

public class RegisterUserCommand : IRequest
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public RegisterUserCommand()
    {
        Username = string.Empty;
        Email = string.Empty;
        Password = string.Empty;
    }

    public RegisterUserCommand(string username, string email, string password)
    {
        Username = username;
        Email = email;
        Password = password;
    }
}

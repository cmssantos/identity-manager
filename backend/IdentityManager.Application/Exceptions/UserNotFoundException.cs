namespace IdentityManager.Application.Exceptions;

public class UserNotFoundException(string message) : Exception(message)
{
}

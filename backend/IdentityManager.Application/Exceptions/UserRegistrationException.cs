using IdentityManager.Domain.Exceptions;
using IdentityManager.Domain.Types;

namespace IdentityManager.Application.Exceptions;

public class UserRegistrationException : CustomException
{
    public UserRegistrationException(ErrorCodes errorCode) : base(errorCode)
    {
    }

    public UserRegistrationException(ErrorCodes errorCode, string message) : base(errorCode, message)
    {
    }
}

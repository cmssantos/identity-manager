using IdentityManager.Domain.Types;

namespace IdentityManager.Domain.Exceptions;

public class CustomArgumentException(ErrorCodes errorCode) : CustomException(errorCode)
{
}

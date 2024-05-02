using IdentityManager.Domain.Types;

namespace IdentityManager.Domain.Exceptions;

public class CustomException : Exception
{
    public ErrorCodes ErrorCode { get; }

    public CustomException(ErrorCodes errorCode)
        : base(errorCode.ToString())
    {
        ErrorCode = errorCode;
    }

    public CustomException(ErrorCodes errorCode, string message)
        : base(message)
    {
        ErrorCode = errorCode;
    }
}

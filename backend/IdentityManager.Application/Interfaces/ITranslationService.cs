using IdentityManager.Domain.Types;

namespace IdentityManager.Application.Interfaces;

public interface ITranslationService
{
    string Translate(ErrorCodes errorCode, string language);
}

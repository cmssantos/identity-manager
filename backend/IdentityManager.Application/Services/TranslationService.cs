using System.Globalization;
using System.Resources;
using IdentityManager.Application.Interfaces;
using IdentityManager.Domain.Types;

namespace IdentityManager.Application.Services;

public class TranslationService : ITranslationService
{
    private readonly ResourceManager _resourceManager;

    public TranslationService()
    {
        _resourceManager = new ResourceManager("IdentityManager.Application.Resources.ErrorMessages", typeof(TranslationService).Assembly);
    }

    public string Translate(ErrorCodes errorCode, string language)
    {
        // Defina a cultura com base no idioma fornecido
        CultureInfo culture = new(language);

        // Obtenha a mensagem do arquivo de recurso correspondente
        string translatedMessage = _resourceManager.GetString(errorCode.ToString(), culture);

        // Retorne a mensagem traduzida, ou um valor padrão se não for encontrada
        return translatedMessage ?? errorCode.ToString();
    }
}

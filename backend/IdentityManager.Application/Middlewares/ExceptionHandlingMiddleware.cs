using System.Text.Json;
using IdentityManager.Application.Exceptions;
using IdentityManager.Application.Interfaces;
using IdentityManager.Domain.Exceptions;
using IdentityManager.Domain.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace IdentityManager.Application.Middlewares;

public class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger,
    ITranslationService translationService)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;
    private readonly ITranslationService _translationService = translationService;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);

        // Sets the HTTP status code based on the exception type
        int statusCode = ex switch
        {
            ArgumentException => StatusCodes.Status400BadRequest,
            UserNotFoundException => StatusCodes.Status404NotFound,
            UserAlreadyExistsException => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        // Determines the language to be used based on the Accept-Language header
        string language = context.Request.Headers["Accept-Language"].FirstOrDefault() ?? "en";

        // Translates the error message based on the ErrorCode
        string errorMessage;
        string errorCodeString = "InternalServerError";

        if (ex is CustomException customException)
        {
            ErrorCodes errorCode = customException.ErrorCode;
            errorMessage = _translationService.Translate(errorCode, language);
            errorCodeString = errorCode.ToString();
        }
        else
        {
            errorMessage = ex.Message;
        }

        // Creates the JSON response with the translated message
        var response = new
        {
            Error = new
            {
                Code = errorCodeString,
                Message = errorMessage
            }
        };

        // Serialize the response to JSON and write to the HTTP response
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}

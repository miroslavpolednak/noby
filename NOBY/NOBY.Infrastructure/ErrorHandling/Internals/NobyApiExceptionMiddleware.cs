using System.Security.Authentication;
using CIS.Infrastructure.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using CIS.Infrastructure.WebApi;
using NOBY.Infrastructure.Security;
using NOBY.Infrastructure.Configuration;
using CIS.Core.Exceptions.ExternalServices;

namespace NOBY.Infrastructure.ErrorHandling.Internals;

public sealed class NobyApiExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILoggerFactory _loggerFactory;

    public NobyApiExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, AppConfiguration appConfiguration)
    {
        var logger = _loggerFactory.CreateLogger<NobyApiExceptionMiddleware>();

        try
        {
            await _next(context);
        }
        // neprihlaseny uzivatel
        catch (CisAuthenticationException ex)
        {
            logger.WebApiAuthenticationException(ex.Message, ex);
            await Results.Json(
                new ApiAuthenticationProblemDetail(ex.ProviderLoginUrl, appConfiguration.Security?.AuthenticationScheme),
                statusCode: 401
                ).ExecuteAsync(context);
        }
        catch (CisAuthorizationException ex)
        {
            logger.WebApiAuthorizationException(ex.Message, ex);
            await Results.Json(singleErrorResult(ex.Message), statusCode: 403).ExecuteAsync(context);
        }
        catch (AuthenticationException ex) // toto by nemelo nastat
        {
            logger.WebApiAuthenticationException(ex.Message, ex);
            await Results.Json(
                new ApiAuthenticationProblemDetail(null, appConfiguration.Security?.AuthenticationScheme),
                statusCode: 401
                ).ExecuteAsync(context);
        }
        catch (NotImplementedException ex)
        {
            logger.WebApiNotImplementedException(ex.Message, ex);
            await Results.Json(singleErrorResult(ex.Message), statusCode: 500).ExecuteAsync(context);
        }
        // object not found
        catch (CisNotFoundException ex)
        {
            logger.NotFoundException(ex.Message, ex);
            await Results.Json(singleErrorResult(ex, 90043), statusCode: 404).ExecuteAsync(context);
        }
        // osetrena validace na urovni FE API
        catch (NobyValidationException ex)
        {
            logger.NobyValidationException(ex.Message, ex);
            await Results.Json(ex.Errors, statusCode: ex.HttpStatusCode).ExecuteAsync(context);
        }
        catch (CisExtServiceValidationException ex)
        {
            logger.NobyValidationException(ex.Message, ex);
            await Results.Json(ex.Errors, statusCode: 400).ExecuteAsync(context);
        }
        // osetrena validace na urovni api call
        catch (CisValidationException ex)
        {
            logger.NobyValidationException(ex.Message, ex);
            await Results.Json(ex.Errors!.Select(t => createErrorItem(parseExceptionCode(t.ExceptionCode), t.Message)), statusCode: 400).ExecuteAsync(context);
        }
        // jakakoliv jina chyba
        catch (Exception ex)
        {
            logger.WebApiUncoughtException(ex);
            await Results.Json(singleErrorResult(NobyValidationException.DefaultExceptionCode, ErrorCodeMapper.Messages[NobyValidationException.DefaultExceptionCode].Message), statusCode: 500).ExecuteAsync(context);
        }
    }

    private static IEnumerable<ApiErrorItem> singleErrorResult(BaseCisException exception, in int? customDefaultExceptionCode = null)
        => singleErrorResult(parseExceptionCode(exception.ExceptionCode), exception.Message, customDefaultExceptionCode);

    private static IEnumerable<ApiErrorItem> singleErrorResult(in string message, in int? customDefaultExceptionCode = null)
        => singleErrorResult(NobyValidationException.DefaultExceptionCode, message, customDefaultExceptionCode);

    private static IEnumerable<ApiErrorItem> singleErrorResult(in int errorCode, in string message, in int? customDefaultExceptionCode = null)
        => new List<ApiErrorItem>
        {
            createErrorItem(errorCode, message, customDefaultExceptionCode)
        };

    private static ApiErrorItem createErrorItem(in int errorCode, in string message, in int? customDefaultExceptionCode = null)
    {
        if (ErrorCodeMapper.DsToApiCodeMapper.ContainsKey(errorCode))
        {
            if (ErrorCodeMapper.DsToApiCodeMapper[errorCode].PropagateDsError)
            {
                int feApiErrorCode = ErrorCodeMapper.DsToApiCodeMapper[errorCode].FeApiCode;
                return new()
                {
                    Severity = ErrorCodeMapper.Messages[feApiErrorCode].Severity,
                    ErrorCode = feApiErrorCode,
                    Message = message
                };
            }
            else
            {
                return createItem(ErrorCodeMapper.DsToApiCodeMapper[errorCode].FeApiCode);
            }    
        }

        if (ErrorCodeMapper.Messages.ContainsKey(errorCode))
        {
            return createItem(errorCode);
        }

        //Unknown exception
        return createItem(customDefaultExceptionCode.HasValue ? customDefaultExceptionCode.Value : NobyValidationException.DefaultExceptionCode);

        static ApiErrorItem createItem(int errorCode)
        {
            return new()
            {
                Severity = ErrorCodeMapper.Messages[errorCode].Severity,
                ErrorCode = errorCode,
                Message = ErrorCodeMapper.Messages[errorCode].Message,
                Description = ErrorCodeMapper.Messages[errorCode].Description
            };
        }
    }

    private static int parseExceptionCode(in string exceptionCode)
        => int.TryParse(exceptionCode, out var code) ? code : NobyValidationException.DefaultExceptionCode;
}

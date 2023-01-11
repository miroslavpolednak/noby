using System.Net;
using System.Security.Authentication;
using CIS.Infrastructure.Logging;
using CIS.Core.Exceptions;
using System.Globalization;
using CIS.Infrastructure.WebApi.Types;

namespace CIS.Infrastructure.WebApi.Middlewares;

public class ApiExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILoggerFactory _loggerFactory;
    
    public ApiExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var logger = _loggerFactory.CreateLogger<ApiExceptionMiddleware>();

        try
        {
            await _next(context);
        }
        // neprihlaseny uzivatel
        catch (AuthenticationException ex)
        {
            logger.WebApiAuthenticationException(ex.Message, ex);
            var model = new ApiAuthenticationProblemDetail
            {
                RedirectUri = "presmerovat_na_caas"
            };
            await Results.Json(model, statusCode: 401).ExecuteAsync(context);
        }
        catch (NotImplementedException ex)
        {
            logger.WebApiNotImplementedException(ex.Message, ex);
            await Results.Json(singleErrorResult("", ex.Message), statusCode: 500).ExecuteAsync(context);
        }
        // DS neni dostupna
        catch (CisServiceUnavailableException ex)
        {
            logger.ExtServiceUnavailable(ex.ServiceName, ex);
            await Results.Json(singleErrorResult("", $"Service '{ex.ServiceName}' unavailable"), statusCode: 500).ExecuteAsync(context);
        }
        // 500 z volane externi sluzby
        catch (CisServiceServerErrorException ex)
        {
            logger.ExtServiceUnavailable(ex.ServiceName, ex);
            await Results.Json(singleErrorResult("", $"Service '{ex.ServiceName}' failed with HTTP 500"), statusCode: 500).ExecuteAsync(context);
        }
        // object not found
        catch (CisNotFoundException ex)
        {
            await Results.Json(singleErrorResult(ex), statusCode: 404).ExecuteAsync(context);
        }
        // conflict 409
        catch (CisConflictException ex)
        {
            await Results.Json(singleErrorResult(ex), statusCode: 409).ExecuteAsync(context);
        }
        // osetrena validace na urovni api call
        catch (CisValidationException ex)
        {
            await Results.Json(castErrors(ex), statusCode: 400).ExecuteAsync(context);
        }
        // jakakoliv jina chyba
        catch (Exception ex)
        {
            logger.WebApiUncoughtException(ex);
            await Results.Json(singleErrorResult("", ex.Message), statusCode: 500).ExecuteAsync(context);
        }
    }


    private static IEnumerable<Types.ApiErrorItem> singleErrorResult(BaseCisException exception)
        => singleErrorResult(exception.ExceptionCode, exception.Message);

    private static IEnumerable<Types.ApiErrorItem> singleErrorResult(string errorCode, string message)
    {
        return new List<Types.ApiErrorItem>
        {
            new Types.ApiErrorItem
            {
                Severity = Types.ApiErrorItemServerity.Error,
                ErrorCode = string.IsNullOrEmpty(errorCode) ? "90001" : errorCode,
                Message = message
            }
        };
    }

    private static IEnumerable<Types.ApiErrorItem> castErrors(CisValidationException ex)
    {
        return ex.Errors!.Select(t => new Types.ApiErrorItem
        {
            Severity = Types.ApiErrorItemServerity.Error,
            ErrorCode = t.Code,
            Message = t.Message
        });
    }
}

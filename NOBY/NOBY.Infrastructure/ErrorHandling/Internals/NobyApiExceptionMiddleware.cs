﻿using System.Security.Authentication;
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
        // DS neni dostupna
        catch (CisServiceUnavailableException ex)
        {
            logger.ExtServiceUnavailable(ex.ServiceName, ex);
            await Results.Json(singleErrorResult($"Service '{ex.ServiceName}' unavailable"), statusCode: 500).ExecuteAsync(context);
        }
        // 500 z volane externi sluzby
        catch (CisServiceServerErrorException ex)
        {
            logger.ExtServiceUnavailable(ex.ServiceName, ex);
            await Results.Json(singleErrorResult($"Service '{ex.ServiceName}' failed with HTTP 500"), statusCode: 500).ExecuteAsync(context);
        }
        // object not found
        catch (CisNotFoundException ex)
        {
            logger.NotFoundException(ex.Message, ex);
            await Results.Json(singleErrorResult(ex), statusCode: 404).ExecuteAsync(context);
        }
        // osetrena validace na urovni FE API
        catch (NobyValidationException ex)
        {
            logger.NobyValidationException(ex.Message, ex);
            await Results.Json(ex.Errors, statusCode: ex.HttpStatusCode).ExecuteAsync(context);
        }
        catch (CisExtServiceValidationException ex)
        {
            await Results.Json(ex.Errors, statusCode: 400).ExecuteAsync(context);
        }
        // osetrena validace na urovni api call
        catch (CisValidationException ex)
        {
            await Results.Json(ex.Errors!.Select(t => createErrorItem(parseExceptionCode(t.ExceptionCode), t.Message)), statusCode: 400).ExecuteAsync(context);
        }
        // jakakoliv jina chyba
        catch (Exception ex)
        {
            logger.WebApiUncoughtException(ex);
            await Results.Json(singleErrorResult(NobyValidationException.DefaultExceptionCode, ErrorCodeMapper.Messages[NobyValidationException.DefaultExceptionCode].Message), statusCode: 500).ExecuteAsync(context);
        }
    }

    private static IEnumerable<ApiErrorItem> singleErrorResult(BaseCisException exception)
        => singleErrorResult(parseExceptionCode(exception.ExceptionCode), exception.Message);

    private static IEnumerable<ApiErrorItem> singleErrorResult(in string message)
        => singleErrorResult(NobyValidationException.DefaultExceptionCode, message);

    private static IEnumerable<ApiErrorItem> singleErrorResult(in int errorCode, in string message)
        => new List<ApiErrorItem>
        {
            createErrorItem(errorCode, message)
        };

    private static ApiErrorItem createErrorItem(in int errorCode, in string message)
    {
        if (ErrorCodeMapper.DsToApiCodeMapper.ContainsKey(errorCode))
        {
            return createItem(ErrorCodeMapper.DsToApiCodeMapper[errorCode]);
        }

        if (ErrorCodeMapper.Messages.ContainsKey(errorCode))
        {
            return createItem(errorCode);
        }

        //Unknown exception
        return createItem(NobyValidationException.DefaultExceptionCode);

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

using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Authentication;
using CIS.Infrastructure.Logging;
using CIS.Core.Exceptions;

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
            logger.GeneralException("ApiExceptionMiddleware", ex.Message, ex);
            await Results.Unauthorized().ExecuteAsync(context);
        }
        catch (NotImplementedException ex)
        {
            logger.GeneralException("ApiExceptionMiddleware", ex.Message, ex);
            await Results.Problem(ex.Message, statusCode: (int)HttpStatusCode.NotImplemented).ExecuteAsync(context);
        }
        // DS neni dostupna
        catch (CisServiceUnavailableException ex)
        {
            logger.ExtServiceUnavailable(ex.ServiceName, ex);
            await Results.Problem(ex.MethodName, $"Service '{ex.ServiceName}' unavailable", statusCode: (int)HttpStatusCode.ServiceUnavailable).ExecuteAsync(context);
        }
        // 500 z volane externi sluzby
        catch (CisServiceServerErrorException ex)
        {
            logger.ExtServiceUnavailable(ex.ServiceName, ex);
            await Results.Problem(ex.MethodName, $"Service '{ex.ServiceName}' failed with HTTP 500", statusCode: (int)HttpStatusCode.FailedDependency).ExecuteAsync(context);
        }
        // serviceCallResult error
        catch (CisServiceCallResultErrorException ex)
        {
            logger.ValidationException(ex.Errors);
            var result = Results.ValidationProblem(ex.Errors.ToDictionary(k => k.Key.ToString(System.Globalization.CultureInfo.InvariantCulture), v => new[] { v.Message }));
            await result.ExecuteAsync(context);
        }
        // object not found
        catch (CisNotFoundException ex)
        {
            logger.EntityNotFound(ex);
            await Results.Problem(ex.Message, statusCode: (int)HttpStatusCode.InternalServerError).ExecuteAsync(context);
        }
        // conflict 409
        catch (Core.Exceptions.CisConflictException ex)
        {
            await Results.Problem(ex.Message, statusCode: 409).ExecuteAsync(context);
        }
        // osetrena validace na urovni api call
        catch (CisValidationException ex)
        {
            IResult result;
            // osetrena validace v pripade, ze se vraci vice validacnich hlasek
            if (ex.ContainErrorsList)
            {
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
                var errors = ex.Errors?.GroupBy(k => k.Key)?.ToDictionary(k => k.Key, v => v.Select(x => x.Message).ToArray()) ?? throw new Core.Exceptions.CisArgumentNullException(15, "Errors collection is empty", "errors");
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
                result = Results.ValidationProblem(errors);
            }
            else if (!string.IsNullOrEmpty(ex.Message))
                result = Results.BadRequest(new ProblemDetails() { Title = ex.Message });
            else
                result = Results.BadRequest(new ProblemDetails() { Title = "Untreated validation exception" });

            logger.GeneralException(ex);
            await result.ExecuteAsync(context);
        }
        // jakakoliv jina chyba
        catch (Exception ex)
        {
            logger.GeneralException(ex);
            await Results.Problem(ex.Message, statusCode: (int)HttpStatusCode.InternalServerError).ExecuteAsync(context);
        }
    }
}

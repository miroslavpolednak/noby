﻿using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Authentication;

namespace CIS.Infrastructure.WebApi.Middlewares;

public class ApiExceptionMiddleware
{
    private readonly RequestDelegate _next;
    
    public ApiExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        // neprihlaseny uzivatel
        catch (AuthenticationException)
        {
            await Results.Unauthorized().ExecuteAsync(context);
        }
        catch (NotImplementedException ex)
        {
            await Results.Problem(ex.Message, statusCode: (int)HttpStatusCode.NotImplemented).ExecuteAsync(context);
        }
        // DS neni dostupna
        catch (Core.Exceptions.ServiceUnavailableException ex)
        {
            await Results.Problem(ex.MethodName, $"Service '{ex.ServiceName}' unavailable", statusCode: (int)HttpStatusCode.ServiceUnavailable).ExecuteAsync(context);
        }
        // serviceCallResult error
        catch (Core.Exceptions.ServiceCallResultErrorException ex)
        {
            var result = Results.ValidationProblem(ex.Errors.ToDictionary(k => k.Key.ToString(System.Globalization.CultureInfo.InvariantCulture), v => new[] { v.Message }));
            await result.ExecuteAsync(context);
        }
        // object not found
        catch (Core.Exceptions.CisNotFoundException ex)
        {
            await Results.Problem(ex.Message, statusCode: (int)HttpStatusCode.InternalServerError).ExecuteAsync(context);
        }
        // osetrena validace na urovni api call
        catch (Core.Exceptions.CisValidationException ex)
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

            await result.ExecuteAsync(context);
        }
        // jakakoliv jina chyba
        catch (Exception ex)
        {
            await Results.Problem(ex.Message, statusCode: (int)HttpStatusCode.InternalServerError).ExecuteAsync(context);
        }
    }
}

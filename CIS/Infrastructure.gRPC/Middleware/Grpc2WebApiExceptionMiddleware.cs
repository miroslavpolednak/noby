using CIS.Core.Exceptions;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Net;
using System.Security.Authentication;

namespace CIS.Infrastructure.gRPC.Middleware;

/// <summary>
/// middleware pro zachytávání vyjímek v grpc code first službách.
/// po přechodu na grpc transcoding ostraníme
/// </summary>
public sealed class Grpc2WebApiExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public Grpc2WebApiExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        
        catch (CisAuthorizationException)
        {
            await Results.Forbid().ExecuteAsync(context);
        }
        // neprihlaseny uzivatel
        catch (AuthenticationException)
        {
            await Results.Unauthorized().ExecuteAsync(context);
        }
        // chyby na strane c4m
        catch (CisServiceUnavailableException ex)
        {
            await Results
                .Problem(ex.Message, title: "External service unavailable", statusCode: (int)HttpStatusCode.FailedDependency, extensions: getResponseExtensions(context))
                .ExecuteAsync(context);
        }
        catch (CisServiceServerErrorException ex)
        {
            await Results.
                Problem(ex.Message, title: "External service server error", statusCode: (int)HttpStatusCode.InternalServerError, extensions: getResponseExtensions(context))
                .ExecuteAsync(context);
        }
        // osetrena validace na urovni api call
        catch (CisValidationException ex)
        {
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
            var errors = ex.Errors?.GroupBy(k => k.ExceptionCode)?.ToDictionary(k => k.Key, v => v.Select(x => x.Message).ToArray());
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
            await getValidationProblemObject(context, errors!);
        }
        catch (CisNotFoundException)
        {
            await Results.NotFound(getResponseExtensions(context)).ExecuteAsync(context);
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.InvalidArgument)
        {
            // try list of errors first
            var messages = ex.GetErrorMessagesFromRpcException();
            if (messages.Any()) // most likely its validation exception
            {
                var errors = messages.GroupBy(k => k.ExceptionCode)?.ToDictionary(k => k.Key, v => v.Select(x => x.Message).ToArray());
                await getValidationProblemObject(context, errors!);
            }
            else // its single argument exception
            {
                await getValidationProblemObject(context, ex.GetErrorMessageFromRpcException(), ex.GetArgumentFromTrailers());
            }
        }
        // jakakoliv jina chyba
        catch (Exception ex)
        {
            await Results
                .Problem(ex.Message, statusCode: (int)HttpStatusCode.InternalServerError, extensions: getResponseExtensions(context))
                .ExecuteAsync(context);
        }
    }

    public static Dictionary<string, object?> getResponseExtensions(HttpContext context)
    {
        return new Dictionary<string, object?>
        {
            { "traceId", Activity.Current?.Id ?? context.TraceIdentifier }
        };
    }

    public static async Task getValidationProblemObject(HttpContext context, string error, string? argument = null)
        => await getValidationProblemObject(context, new Dictionary<string, string[]> { { argument ?? "0", new[] { error } } });

    public static async Task getValidationProblemObject(HttpContext context, Dictionary<string, string[]> errors)
    {
        await Results.ValidationProblem(
            errors,
            title: "One or more validation errors occurred.",
            extensions: getResponseExtensions(context))
            .ExecuteAsync(context!);
    }
}

using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Authentication;

namespace CIS.Infrastructure.gRPC.Middleware;

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
        // neprihlaseny uzivatel
        catch (AuthenticationException)
        {
            await Results.Unauthorized().ExecuteAsync(context);
        }
        // chyby na strane c4m
        catch (Core.Exceptions.CisServiceUnavailableException ex)
        {
            await Results.Problem(ex.Message, title: "External service unavailable", statusCode: (int)HttpStatusCode.FailedDependency).ExecuteAsync(context);
        }
        catch (Core.Exceptions.CisServiceServerErrorException ex)
        {
            await Results.Problem(ex.Message, title: "External service server error", statusCode: (int)HttpStatusCode.FailedDependency).ExecuteAsync(context);
        }
        // osetrena validace na urovni api call
        catch (Core.Exceptions.CisExtServiceValidationException ex)
        {
            IResult result;
            // osetrena validace v pripade, ze se vraci vice validacnich hlasek
            if (ex.ContainErrorsList)
            {
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
                var errors = ex.Errors?.GroupBy(k => k.Key)?.ToDictionary(k => k.Key, v => v.Select(x => x.Message).ToArray()) ?? throw new Core.Exceptions.CisArgumentNullException(15, "Errors collection is empty", "errors");
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
                result = Results.ValidationProblem(errors, title: "External service validation failed", detail: ex.Message);
            }
            else if (!string.IsNullOrEmpty(ex.Message))
                result = Results.BadRequest(new ProblemDetails() 
                { 
                    Title = "External service validation failed", 
                    Detail = ex.Message 
                });
            else
                result = Results.BadRequest(new ProblemDetails() { Title = "External service: Untreated validation exception" });

            await result.ExecuteAsync(context);
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
                result = Results.ValidationProblem(errors, title: ex.Message);
            }
            else if (!string.IsNullOrEmpty(ex.Message))
                result = Results.ValidationProblem(new Dictionary<string, string[]> { { "0", new[] { ex.Message } } }, title: "One or more validation errors occurred.");
            else
                result = Results.BadRequest(new ProblemDetails() { Title = "Untreated validation exception" });

            await result.ExecuteAsync(context);
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.InvalidArgument)
        {
            IResult result;
            // try list of errors first
            var messages = ex.GetErrorMessagesFromRpcException();
            if (messages.Any()) // most likely its validation exception
            {
                var errors = messages.GroupBy(k => k.Key)?.ToDictionary(k => k.Key, v => v.Select(x => x.Message).ToArray());
                result = Results.ValidationProblem(errors!);
            }
            else // its single argument exception
            {
                string message = ex.GetErrorMessageFromRpcException();
                result = Results.BadRequest(new ProblemDetails() { Title = ex.Message });
            }
            await result.ExecuteAsync(context);
        }
        // jakakoliv jina chyba
        catch (Exception ex)
        {
            await Results.Problem(ex.Message, statusCode: (int)HttpStatusCode.InternalServerError).ExecuteAsync(context);
        }
    }
}

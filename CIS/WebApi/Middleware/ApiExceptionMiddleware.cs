using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;

namespace CIS.Infrastructure.WebApi.Middlewares;

public class ApiExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILoggerFactory _loggerFactory;

    public ApiExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
    {
        _next = next;
        _loggerFactory = loggerFactory;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (AuthenticationException)
        {
            await Results.Unauthorized().ExecuteAsync(context);
        }
        // osetrena validace na urovni api call
        catch (Core.Exceptions.CisValidationException ex)
        {
            IResult result;
            // osetrena validace v pripade, ze se vraci vice validacnich hlasek
            if (ex.ContainErrorsList)
            {
                var errors = ex.Errors?.GroupBy(k => k.Key)?.ToDictionary(k => k.Key, v => v.Select(x => x.Message).ToArray()) ?? throw new ArgumentNullException("errors");
                result = Results.ValidationProblem(errors);
            }
            else if (!string.IsNullOrEmpty(ex.Message))
                result = Results.BadRequest(new ProblemDetails() { Title = ex.Message });
            else
                result = Results.BadRequest(new ProblemDetails() { Title = "Untreated validation exception" });

            await result.ExecuteAsync(context);
        }
    }
}

using System.Diagnostics;
using System.Reflection;

namespace CIS.Infrastructure.WebApi.Middleware;

public class TraceIdResponseHeaderMiddleware(RequestDelegate _next)
{
    private static readonly string _appVersion = Assembly.GetEntryAssembly()!.GetName().Version?.ToString() ?? "unknown";


    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.OnStarting(() =>
        {
            context.Response.Headers.Append("trace-id", Activity.Current?.Id ?? context.TraceIdentifier);
            context.Response.Headers.Append("api-ver", _appVersion);

            return Task.CompletedTask;
        });

        await _next(context);
    }
}

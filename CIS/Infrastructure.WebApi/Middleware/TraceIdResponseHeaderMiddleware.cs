using System.Diagnostics;
using System.Reflection;

namespace CIS.Infrastructure.WebApi.Middleware;

public class TraceIdResponseHeaderMiddleware
{
    static string _appVersion = "";

    static TraceIdResponseHeaderMiddleware()
    {
        _appVersion = Assembly.GetEntryAssembly()!.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion;
    }

    private readonly RequestDelegate _next;

    public TraceIdResponseHeaderMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.OnStarting(() =>
        {
            context.Response.Headers.Add("trace-id", Activity.Current?.Id ?? context.TraceIdentifier);
            context.Response.Headers.Add("api-ver", _appVersion);

            return Task.CompletedTask;
        });

        await _next(context);
    }
}

using System.Diagnostics;

namespace CIS.Infrastructure.WebApi.Middleware;

public class TraceIdResponseHeaderMiddleware
{
    private readonly RequestDelegate _next;

    public TraceIdResponseHeaderMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var headers = context.Response.Headers;
        
        headers.Add("trace-id", Activity.Current?.Id ?? context.TraceIdentifier);
        
        await _next(context);
    }
}

namespace CIS.Infrastructure.WebApi.Middleware;

/// <summary>
/// Pro HTTP metodu OPTIONS vrací 204 + patřičné hlavičky dle RFC 7231
/// </summary>
public sealed class HttpOptionsMiddleware(RequestDelegate _next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);

        if (context.Request.Method == "OPTIONS" && context.Response.StatusCode == StatusCodes.Status405MethodNotAllowed)
        {
            context.Response.StatusCode = StatusCodes.Status204NoContent;
            context.Response.Headers["Allow"] += ", OPTIONS";
        }
    }
}

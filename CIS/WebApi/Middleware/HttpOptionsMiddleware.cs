namespace CIS.Infrastructure.WebApi.Middleware;

/// <summary>
/// Pro HTTP metodu OPTIONS vrací 204 + patřičné hlavičky dle RFC 7231
/// </summary>
public class HttpOptionsMiddleware
{
    private readonly RequestDelegate next;

    public HttpOptionsMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await next(context);

        if (context.Request.Method == "OPTIONS" && context.Response.StatusCode == StatusCodes.Status405MethodNotAllowed)
        {
            context.Response.StatusCode = StatusCodes.Status204NoContent;
            context.Response.Headers["Allow"] += ", OPTIONS";
        }
    }
}

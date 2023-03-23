namespace CIS.Infrastructure.WebApi;

public static class CisSecurityHeaders
{
    public static IApplicationBuilder UseCisSecurityHeaders(this IApplicationBuilder app)
    {
        app.UseMiddleware<Middleware.HttpOptionsMiddleware>();

        app.UseCisWebApiCors();

        app.UseHsts();

        app.UseHttpsRedirection();

        // CSP
        app.Use(async (context, next) => {
            context.Response.OnStarting(() => {
                context.Response.Headers.Add("Content-Security-Policy", "default-src 'none'; frame-ancestors 'none'");
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");

                return Task.CompletedTask;
            });

            await next();
        });

        return app;
    }
}

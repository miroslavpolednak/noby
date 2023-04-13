using NOBY.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using NOBY.Infrastructure.Configuration;
using CIS.Infrastructure.WebApi;

namespace NOBY.Api.StartupExtensions;

internal static class NobyAppBuilder
{
    public static IApplicationBuilder UseNobySpa(this IApplicationBuilder app)
        => app.MapWhen(_isSpaCall, appBuilder => 
        {
            appBuilder.UseSpaStaticFiles();
            appBuilder.UseSpa(spa =>
            {
                spa.Options.SourcePath = "wwwroot";
            });
        });

    public static IApplicationBuilder UseNobyHealthChecks(this IApplicationBuilder app)
        => app.MapWhen(_isHealthCheck, appBuilder =>
        {
            appBuilder.UseRouting();
            appBuilder.UseEndpoints(endpoints =>
            {
                endpoints.MapCisHealthChecks();
            });
        });

    public static IApplicationBuilder UseNobyApi(this WebApplication app, AppConfiguration appConfiguration)
        => app.MapWhen(_isApiCall, appBuilder =>
        {
            appBuilder.UseHttpLogging();
            appBuilder.UseMiddleware<CIS.Infrastructure.WebApi.Middleware.TraceIdResponseHeaderMiddleware>();
            appBuilder.UseCisSecurityHeaders();

            // detailed error page
            if (appConfiguration.UseDeveloperExceptionPage)
            {
                appBuilder.UseDeveloperExceptionPage();
            }
            else // custom exception handling
            {
                appBuilder.UseMiddleware<Infrastructure.ErrorHandling.Internals.NobyApiExceptionMiddleware>();
            }

            // autentizace a autorizace
            appBuilder.UseAuthentication();
            appBuilder.UseMiddleware<AppSecurityMiddleware>();
            appBuilder.UseAuthorization();
            
            // namapovani API modulu
            appBuilder
                .UseRouting()
                .UseEndpoints(t =>
                {
                    t.MapControllers();
                });
        });

    /// <summary>
    /// routy pro autentizaci a signout uzivatele
    /// </summary>
    public static IApplicationBuilder UseNobyAuthStrategy(this IApplicationBuilder app)
        => app
        .UseWhen(_isAuthCall, (appBuilder) =>
        {
            appBuilder.UseRouting();

            appBuilder.UseAuthentication();
            appBuilder.UseAuthorization();

            appBuilder.UseEndpoints(t =>
            {
                // sign in
                t.MapGet(AuthenticationConstants.DefaultAuthenticationUrlPrefix + AuthenticationConstants.DefaultSignInEndpoint, ([FromServices] IHttpContextAccessor context) =>
                {
                })
                    .RequireAuthorization()
                    .WithDescription("Přihlášení uživatele / redirect na auth provider.")
                    .WithTags("Authentication")
                    .WithOpenApi();

                // sign out
                t.MapGet(AuthenticationConstants.DefaultAuthenticationUrlPrefix + AuthenticationConstants.DefaultSignOutEndpoint, 
                    ([FromServices] IHttpContextAccessor context, [FromServices] AppConfiguration configuration) =>
                {
                    context.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                    if (configuration.Security!.AuthenticationScheme == AuthenticationConstants.CaasAuthScheme)
                    {
                        context.HttpContext!.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
                    }

                    // redirect to root?
                    context.HttpContext!.Response.Redirect("/");
                })
                    .RequireAuthorization()
                    .WithDescription("Odhlášení uživatele.")
                    .WithTags("Authentication")
                    .WithOpenApi();
            });
        });

    public static IApplicationBuilder UseNobySwagger(this IApplicationBuilder app)
        => app
        .UseSwagger()
        .UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "NOBY FRONTEND API");
        });

    private static readonly Func<HttpContext, bool> _isApiCall = (HttpContext context) 
        => context.Request.Path.StartsWithSegments("/api");

    private static readonly Func<HttpContext, bool> _isAuthCall = (HttpContext context)
        => context.Request.Path.StartsWithSegments(AuthenticationConstants.DefaultAuthenticationUrlSegment);

    private static readonly Func<HttpContext, bool> _isHealthCheck = (HttpContext context) 
        => context.Request.Path.StartsWithSegments(CIS.Core.CisGlobalConstants.CisHealthCheckEndpointUrl);

    private static readonly Func<HttpContext, bool> _isSpaCall = (HttpContext context) 
        => !context.Request.Path.StartsWithSegments(AuthenticationConstants.DefaultAuthenticationUrlSegment) 
            && !context.Request.Path.StartsWithSegments("/api") 
            && !context.Request.Path.StartsWithSegments("/swagger") 
            && !context.Request.Path.StartsWithSegments(CIS.Core.CisGlobalConstants.CisHealthCheckEndpointUrl);       
}
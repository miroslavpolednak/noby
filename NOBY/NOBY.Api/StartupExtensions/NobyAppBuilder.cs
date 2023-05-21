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
                    .Produces(302)
                    .WithDescription("Přihlášení uživatele / redirect na auth provider.")
                    .WithTags("Users")
                    .WithName("loginUserGet")
                    .WithOpenApi(generatedOperation =>
                    {
                        generatedOperation.Summary = "Přihlášení uživatele / redirect na auth provider.";
                        generatedOperation.Responses.First().Value.Description = "Redirect na CAAS";
                        return generatedOperation;
                    });

                // Odhlášení přihlášeného uživatele
                t.MapGet(AuthenticationConstants.DefaultAuthenticationUrlPrefix + AuthenticationConstants.DefaultSignOutEndpoint, 
                    ([FromServices] IHttpContextAccessor context, [FromServices] AppConfiguration configuration, [FromQuery] string? redirect) =>
                {
                    string redirectUrl = Uri.TryCreate(redirect, UriKind.Absolute, out var uri) ? uri.ToString() : "/";

                    context.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                    if (configuration.Security!.AuthenticationScheme == AuthenticationConstants.CaasAuthScheme)
                    {
                        context.HttpContext!.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
                    }

                    // redirect to root?
                    context.HttpContext!.Response.Redirect(redirectUrl);
                })
                    .RequireAuthorization()
                    .Produces(302)
                    .WithTags("Users")
                    .WithName("signoutUserGet")
                    .WithOpenApi(generatedOperation =>
                    {
                        generatedOperation.Summary = "Odhlášení přihlášeného uživatele";
                        generatedOperation.Description = "Provolání zajistí správné odhlášení přihlášeného uživatele. <br /><br /><a href=\"https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=713A5C7F-13DB-4c88-863B-29C40F2EDE32\"><img src=\"https://eacloud.ds.kb.cz/webea/images/element64/diagramsequence.png\" width=\"20\" height=\"20\" />Diagram v EA</a>";
                        
                        var parameter = generatedOperation.Parameters[0];
                        parameter.Description = "Absolutní URI kam má být uživatel přesměrován po odhlášení";

                        generatedOperation.Responses.First().Value.Description = "Uživatel byl ohlášen";

                        return generatedOperation;
                    });
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
using CIS.Infrastructure.StartupExtensions;
using System.Reflection;
using NOBY.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;

namespace NOBY.Api.StartupExtensions;

internal static class NobyAppBuilder
{
    static string _appVersion = "";

    static NobyAppBuilder()
    {
        _appVersion = Assembly.GetEntryAssembly()!.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion;
    }

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

    public static IApplicationBuilder UseNobyApi(this WebApplication app)
        => app.MapWhen(_isApiCall, appBuilder =>
        {
            appBuilder.UseHttpLogging();
            appBuilder.UseCisWebApiCors();

            // error middlewares
            if (app.Environment.IsDevelopment())
            {
                appBuilder.UseDeveloperExceptionPage();
            }
            else // custom exception handling
            {
                appBuilder.UseMiddleware<Infrastructure.ErrorHandling.Internals.NobyApiExceptionMiddleware>();
                appBuilder.UseHsts();
            }

            // version header
            appBuilder.Use(async (context, next) =>
            {
                context.Response.Headers.Add("foms-ver", _appVersion);
                await next();
            });

            appBuilder.UseMiddleware<CIS.Infrastructure.WebApi.Middleware.HttpOptionsMiddleware>();

            // autentizace a autorizace
            appBuilder.UseAuthentication();
            appBuilder.UseMiddleware<AppSecurityMiddleware>();
            appBuilder.UseAuthorization();
            appBuilder.UseMiddleware<CIS.Infrastructure.WebApi.Middleware.TraceIdResponseHeaderMiddleware>();

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
                t.MapGet(AuthenticationConstants.DefaultAuthenticationUrlPrefix + AuthenticationConstants.DefaultSignInEndpoint, ([FromServices] IHttpContextAccessor context) =>
                {
                    context.HttpContext!.Response.Redirect("/#");
                })
                    .RequireAuthorization()
                    .ExcludeFromDescription();

                t.MapGet(AuthenticationConstants.DefaultAuthenticationUrlPrefix + AuthenticationConstants.DefaultSignOutEndpoint, ([FromServices] IHttpContextAccessor context) =>
                {
                    context.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    context.HttpContext!.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
                })
                    .RequireAuthorization()
                    .ExcludeFromDescription();
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
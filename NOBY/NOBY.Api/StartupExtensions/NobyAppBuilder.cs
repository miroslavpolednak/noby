using NOBY.Infrastructure.Security.Endpoints;
using NOBY.Infrastructure.Configuration;
using CIS.Infrastructure.WebApi;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace NOBY.Api.StartupExtensions;

internal static class NobyAppBuilder
{
    public static IApplicationBuilder UseNobySpa(this IApplicationBuilder app)
        => app.MapWhen(_isSpaCall, appBuilder => 
        {
            var corsService = appBuilder.ApplicationServices.GetRequiredService<ICorsService>();
            var corsPolicyProvider = appBuilder.ApplicationServices.GetRequiredService<ICorsPolicyProvider>();
            
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
            appBuilder.UseMiddleware<NobySecurityMiddleware>();
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
            appBuilder.MapNobyAuthenticationEndpoints();
        });

    public static IApplicationBuilder UseNobySwagger(this IApplicationBuilder app)
        => app
        .UseSwagger()
        .UseSwaggerUI(c =>
        {
            c.DisplayOperationId();
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "NOBY FRONTEND API");
        });

    public static IApplicationBuilder UseRedirectStrategy(this IApplicationBuilder app)
        => app
        .UseWhen(_isRedirectCall, (appBuilder) =>
        {
            appBuilder.Run(async context =>
            {
                var url = context.Request.GetEncodedUrl();
                var idx = url.IndexOf('/', 10);
                context.Response.Redirect(string.Concat(url[..idx], "/#", url[idx..]));
            });
        });

    private static readonly Func<HttpContext, bool> _isApiCall = (HttpContext context) 
        => context.Request.Path.StartsWithSegments("/api");

    private static readonly Func<HttpContext, bool> _isRedirectCall = (HttpContext context)
        => context.Request.Path.StartsWithSegments("/redirect");

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
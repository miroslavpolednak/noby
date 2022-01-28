using CIS.Infrastructure.StartupExtensions;
using System.Reflection;

namespace FOMS.Api.StartupExtensions;

internal static class FomsAppBuilder
{
    public static IApplicationBuilder UseFomsSpa(this IApplicationBuilder app)
        => app.MapWhen(_isSpaCall, appBuilder => 
        {
            appBuilder.UseSpaStaticFiles();
            appBuilder.UseSpa(spa =>
            {
                spa.Options.SourcePath = "wwwroot";
            });
        });

    public static IApplicationBuilder UseFomsHealthChecks(this IApplicationBuilder app)
        => app.MapWhen(_isHealthCheck, appBuilder =>
        {
            appBuilder.MapCisHealthChecks();
        });

    public static IApplicationBuilder UseFomsApi(this WebApplication app)
        => app.MapWhen(_isApiCall, appBuilder =>
        {
            // error middlewares
            /*if (app.Environment.IsProduction())
                appBuilder.UseExceptionHandler("/error");
            else*/
            appBuilder.UseDeveloperExceptionPage();

            // exception handling
            appBuilder.UseMiddleware<CIS.Infrastructure.WebApi.Middlewares.ApiExceptionMiddleware>();

            if (app.Environment.IsProduction())
                appBuilder.UseHsts();

            appBuilder.UseCors();
            appBuilder.UseMiddleware<CIS.Infrastructure.WebApi.Middleware.HttpOptionsMiddleware>();

            // autentizace a autorizace
            appBuilder.UseAuthentication();
            appBuilder.UseMiddleware<FOMS.Infrastructure.Security.AppSecurityMiddleware>();

            // namapovani API modulu
            appBuilder.AddEndpointsModules(typeof(FOMS.Api.IApiAssembly).GetTypeInfo().Assembly);
        });

    public static IApplicationBuilder UseFomsSwagger(this IApplicationBuilder app)
        => app
        .UseSwagger()
        .UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "NOBY FRONTEND API");
        });

    private static Func<HttpContext, bool> _isApiCall = (HttpContext context) 
        => context.Request.Path.StartsWithSegments("/api");

    private static Func<HttpContext, bool> _isHealthCheck = (HttpContext context) 
        => context.Request.Path.StartsWithSegments(CisHealthChecks.HealthCheckEndpoint);

    private static Func<HttpContext, bool> _isSpaCall = (HttpContext context) 
        => !context.Request.Path.StartsWithSegments("/api") && !context.Request.Path.StartsWithSegments("/swagger") && !context.Request.Path.StartsWithSegments(CisHealthChecks.HealthCheckEndpoint);       
}
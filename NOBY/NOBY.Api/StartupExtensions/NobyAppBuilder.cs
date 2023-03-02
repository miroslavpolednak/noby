﻿using CIS.Infrastructure.StartupExtensions;
using CIS.Infrastructure.Telemetry;
using System.Reflection;
using NOBY.Infrastructure.Security;

namespace NOBY.Api.StartupExtensions;

internal static class NobyAppBuilder
{
    static string _appVersion = "";

    static NobyAppBuilder()
    {
        _appVersion = Assembly.GetEntryAssembly()!.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion;
    }

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
            appBuilder.UseRouting();
            appBuilder.UseEndpoints(endpoints =>
            {
                endpoints.MapCisHealthChecks();
            });
        });

    public static IApplicationBuilder UseFomsApi(this WebApplication app)
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
                appBuilder.UseMiddleware<NOBY.Infrastructure.ErrorHandling.NobyApiExceptionMiddleware>();
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
            appBuilder.UseCisLogging();
            appBuilder.UseMiddleware<CIS.Infrastructure.WebApi.Middleware.TraceIdResponseHeaderMiddleware>();

            // namapovani API modulu
            appBuilder
                .UseRouting()
                .UseEndpoints(t =>
                {
                    t.MapControllers();
                });
        });

    public static IApplicationBuilder UseFomsSwagger(this IApplicationBuilder app)
        => app
        .UseSwagger()
        .UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "NOBY FRONTEND API");
        });

    private static readonly Func<HttpContext, bool> _isApiCall = (HttpContext context) 
        => context.Request.Path.StartsWithSegments("/api");

    private static readonly Func<HttpContext, bool> _isHealthCheck = (HttpContext context) 
        => context.Request.Path.StartsWithSegments(CIS.Core.CisGlobalConstants.CisHealthCheckEndpointUrl);

    private static readonly Func<HttpContext, bool> _isSpaCall = (HttpContext context) 
        => !context.Request.Path.StartsWithSegments("/api") && !context.Request.Path.StartsWithSegments("/swagger") && !context.Request.Path.StartsWithSegments(CIS.Core.CisGlobalConstants.CisHealthCheckEndpointUrl);       
}
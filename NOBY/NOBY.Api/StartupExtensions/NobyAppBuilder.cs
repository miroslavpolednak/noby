﻿using NOBY.Infrastructure.Security.Endpoints;
using NOBY.Infrastructure.Configuration;
using CIS.Infrastructure.WebApi;
using Microsoft.AspNetCore.Http.Extensions;
using NOBY.Infrastructure.Security.Middleware;
using Asp.Versioning.ApiExplorer;
using Microsoft.FeatureManagement;
using SharedTypes;

namespace NOBY.Api.StartupExtensions;

internal static class NobyAppBuilder
{
    public static IApplicationBuilder UseNobySpa(this IApplicationBuilder app)
        => app.MapWhen(_isSpaCall, appBuilder =>
        {
            appBuilder.UseSpaStaticFiles();
            appBuilder.UseStaticFiles("/docs");
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
            
            // namapovani API modulu - !poradi je dulezite
            appBuilder
                // autentizace
                .UseAuthentication()
                // routing
                .UseRouting()
                .UseWhen(context =>
                         {
                             var featureManager = context.RequestServices.GetRequiredService<IFeatureManager>();

                             return featureManager.IsEnabledAsync(FeatureFlagsConstants.LogRequestContractDifferences).Result;
                         },
                         builder => builder.UseMiddleware<CIS.Infrastructure.WebApi.Middleware.RequestBufferingMiddleware>())
                // autorizace
                .UseMiddleware<NobySecurityMiddleware>()
                .UseAuthorization()
                .UseMiddleware<CaseOwnerValidationMiddleware>()
                // endpointy
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

    public static IApplicationBuilder UseNobySwagger(this IApplicationBuilder app, IReadOnlyList<ApiVersionDescription> descriptions)
        => app
        .UseSwagger()
        .UseSwaggerUI(c =>
        {
            // build a swagger endpoint for each discovered API version
            foreach (var description in descriptions)
            {
                var url = $"/swagger/{description.GroupName}/swagger.json";
                var name = description.GroupName.ToUpperInvariant();
                c.SwaggerEndpoint(url, name);
            }

            c.DisplayOperationId();
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
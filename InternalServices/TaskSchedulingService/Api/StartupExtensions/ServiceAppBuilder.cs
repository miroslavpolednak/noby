using CIS.Infrastructure.WebApi;

namespace CIS.InternalServices.TaskSchedulingService.Api.StartupExtensions;

internal static class ServiceAppBuilder
{
    public static IApplicationBuilder AddSchedulerUI(this IApplicationBuilder app)
    {
        return app;
    }

    public static IApplicationBuilder UseServiceHealthChecks(this IApplicationBuilder app)
        => app.MapWhen(_isHealthCheck, appBuilder =>
        {
            appBuilder.UseRouting();
            appBuilder.UseEndpoints(endpoints =>
            {
                endpoints.MapCisHealthChecks();
            });
        });

    private static readonly Func<HttpContext, bool> _isHealthCheck = (HttpContext context)
        => context.Request.Path.StartsWithSegments(CIS.Core.CisGlobalConstants.CisHealthCheckEndpointUrl);
}

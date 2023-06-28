using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CIS.Infrastructure.WebApi;

public static class CisHealthChecks
{
    public static IHealthChecksBuilder AddCisHealthChecks(this WebApplicationBuilder builder)
    {
        // base hc
        var hc = builder.Services.AddHealthChecks();

        // health check na databaze podle dostupnych connection stringu
        var section = builder.Configuration.GetSection("ConnectionStrings")?.GetChildren();
        if (section != null)
        {
            var elements = section.Where(t => !string.IsNullOrEmpty(t.Value)).ToArray();
            foreach (var cs in elements)
                hc.AddSqlServer(cs.Value!, name: cs.Key);
        }

        return hc;
    }

    public static IEndpointConventionBuilder MapCisHealthChecks(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapHealthChecks(CIS.Core.CisGlobalConstants.CisHealthCheckEndpointUrl, new HealthCheckOptions
        {
            ResultStatusCodes =
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Degraded] = StatusCodes.Status200OK,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
            }
        });
    }
}

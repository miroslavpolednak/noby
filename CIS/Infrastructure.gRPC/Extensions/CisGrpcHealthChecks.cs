using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CIS.Infrastructure.gRPC;

public static class CisGrpcHealthChecks
{
    /// <summary>
    /// Zaregistruje health checky pro gRPC sluzby. Pridava healthcheck na sluzbu jako takovou a databaze v ni pouzite.
    /// </summary>
    public static IHealthChecksBuilder AddCisGrpcHealthChecks(this WebApplicationBuilder builder)
    {
        // base check
        var hc = builder.Services
            .AddGrpcHealthChecks()
            .AddCheck("Service", () => HealthCheckResult.Healthy());

        // pridat check na databazi
        var section = builder.Configuration.GetSection("ConnectionStrings")?.GetChildren();
        if (section != null)
        {
            var elements = section.Where(t => !string.IsNullOrEmpty(t.Value)).ToArray();
            foreach (var cs in elements)
                hc.AddSqlServer(cs.Value!, name: cs.Key);
        }

        return hc;
    }

    /// <summary>
    /// Mapuje gRPC healthcheck endpoint.
    /// </summary>
    public static void MapCisGrpcHealthChecks(this IEndpointRouteBuilder app)
    {
        app.MapGrpcHealthChecksService();
    }
}

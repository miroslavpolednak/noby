using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CIS.Infrastructure.gRPC;

public static class HealthChecksExtensions
{
    public static void AddCisGrpcHealthChecks(this WebApplicationBuilder builder)
    {
        builder.Services.AddGrpcHealthChecks(configure =>
        {
            configure.Services.MapService
        })
                .AddCheck("health1", () => HealthCheckResult.Healthy("nene"))
                .AddCheck("health2", () => HealthCheckResult.Healthy("ahoj"));
    }

    public static void MapCisGrpcHealthChecks(this IEndpointRouteBuilder app)
    {
        app.MapGrpcHealthChecksService();
    }
}

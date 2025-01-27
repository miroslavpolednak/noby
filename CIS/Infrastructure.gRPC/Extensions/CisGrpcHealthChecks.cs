﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CIS.Infrastructure.gRPC;

public static class CisGrpcHealthChecks
{
    /// <summary>
    /// Zaregistruje health checky pro gRPC sluzby + registruje healthcheck i pro HTTP1.1.
    /// </summary>
    /// <remarks>
    /// Pridava healthcheck na sluzbu jako takovou a databaze v ni pouzite.
    /// </remarks>
    public static IHealthChecksBuilder AddCisGrpcHealthChecks(this WebApplicationBuilder builder)
    {
        // pridat http1 healthcheck kvuli F5
        builder.Services.AddHealthChecks();

        // base check
        var hc = builder.Services
            .AddGrpcHealthChecks()
            .AddCheck("Service", () => HealthCheckResult.Healthy());

        // pridat check na databazi
        //TODO tohle neni uplne spravne - potrebujeme pridavat CS podle toho, jakeho jsou typu: MsSql, Redis atd.
        //TODO jak to vyresit? Hledat instance DbCOntextu a podle toho? Nebo regexem?
        var section = builder.Configuration.GetSection("ConnectionStrings")?.GetChildren();
        if (section != null)
        {
            var elements = section.Where(t => !string.IsNullOrEmpty(t.Value) && t.Key != "cisDistributedCache").ToArray();
            foreach (var cs in elements)
            {
                hc.AddSqlServer(cs.Value!, name: cs.Key);
            }
        }

        // nepublikovat automaticky, delame to obracene
        builder.Services.Configure<HealthCheckPublisherOptions>(options =>
        {
            options.Predicate = reg => false;
            options.Delay = TimeSpan.FromDays(2);
            options.Period = TimeSpan.FromDays(2);
        });

        return hc;
    }

    /// <summary>
    /// Mapuje gRPC a HTTP1.1 healthcheck endpoint.
    /// </summary>
    public static IEndpointRouteBuilder MapCisGrpcHealthChecks(this IEndpointRouteBuilder app)
    {
        // registrace grpc endpointu
        app.MapGrpcHealthChecksService();

        // registrace http1 endpointu
        app.MapHealthChecks(CIS.Core.CisGlobalConstants.CisHealthCheckEndpointUrl, new HealthCheckOptions
        {
            ResultStatusCodes =
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Degraded] = StatusCodes.Status200OK,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
            }
        });

        return app;
    }
}

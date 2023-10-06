﻿using Microsoft.AspNetCore.Builder;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace CIS.Infrastructure.Telemetry;

/// <summary>
/// Extension metody do startupu aplikace pro registraci tracingu.
/// </summary>
public static class TracingExtensions
{
    /// <summary>
    /// Register Open Tracing instrumentation
    /// </summary>
    /// <param name="serviceName">Nazev sluzby, ktery se zobrazi v exporteru. Pokud neni zadano, hleda se v ICisEnvironmentConfiguration[DefaultApplicationKey]</param>
    public static WebApplicationBuilder AddCisTracing(this WebApplicationBuilder builder, string? serviceName = null)
    {
        builder.Services.AddOpenTelemetry()
            .WithTracing(b =>
            {
                // set service name
                /*if (string.IsNullOrEmpty(serviceName))
                {
                    var environmentConfiguration = b.GetServices().FirstOrDefault(t => t.ServiceType.Name == "ICisEnvironmentConfiguration")?.ImplementationInstance;
                    if (environmentConfiguration is not null)
                        b.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(((Core.Configuration.ICisEnvironmentConfiguration)environmentConfiguration).DefaultApplicationKey));
                }
                else
                {
                    b.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName));
                }*/

                // receive traces from built-in sources
                b.AddEntityFrameworkCoreInstrumentation();
                b.AddSqlClientInstrumentation();
                b.AddHttpClientInstrumentation();
                b.AddAspNetCoreInstrumentation();
                b.AddGrpcClientInstrumentation(options => options.SuppressDownstreamInstrumentation = true);
            });

        return builder;
    }
}

using CIS.Core.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Metrics;
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
    public static WebApplicationBuilder AddCisTracing(this WebApplicationBuilder builder)
    {
        // get configuration from json file
        var configuration = builder.Configuration
            .GetSection(_configurationTelemetryKey)
            .Get<TracingConfiguration>();
        
        if (configuration?.Connection is null || configuration.Provider == TracingProviders.None)
        {
            return builder;
        }

        // musim takhle, protoze OT registrace neumoznuje nijak pristup na service provider
        var envConfiguration = builder.Configuration
            .GetSection(Core.CisGlobalConstants.EnvironmentConfigurationSectionName)
            .Get<CisEnvironmentConfiguration>();

        builder.Services.AddSingleton(configuration);

        switch (configuration.Provider)
        {
            case TracingProviders.OpenTelemetry:
                builder
                    .Services
                    .AddOpenTelemetry()
                    .ConfigureResource(res =>
                    {
                        res.AddService(envConfiguration!.DefaultApplicationKey!);
                        res.AddAttributes(new List<KeyValuePair<string, object>>
                        {
                            new ("CisEnvironment", envConfiguration.EnvironmentName!)
                        });
                    })
                    .WithTracing(tracing =>
                    {
                        tracing
                            .AddEntityFrameworkCoreInstrumentation()
                            .AddSqlClientInstrumentation()
                            .AddHttpClientInstrumentation()
                            .AddAspNetCoreInstrumentation()
                            .AddWcfInstrumentation()
                            .AddGrpcClientInstrumentation(options => options.SuppressDownstreamInstrumentation = true)
                            .AddOtlpExporter(opts =>
                            {
                                opts.Endpoint = new Uri(configuration.Connection!.Url!);
                            });

                    })
                    .WithMetrics(metrics =>
                    {
                        metrics
                          .AddAspNetCoreInstrumentation()
                          .AddHttpClientInstrumentation()
                          .AddOtlpExporter(opts =>
                          {
                              opts.Endpoint = new Uri(configuration.Connection!.Url!);
                          });
                    });
                break;
        }
        

        return builder;
    }

    internal const string _configurationTelemetryKey = "CisTelemetry:Tracing";
}

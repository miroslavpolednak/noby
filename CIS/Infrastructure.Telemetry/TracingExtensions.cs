using CIS.Core.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Exporter;
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
        
        if (configuration is null
            || (string.IsNullOrWhiteSpace(configuration.OTLP?.CollectorUrl) && !configuration.UseConsole))
        {
            return builder;
        }
        
        // musim takhle, protoze OT registrace neumoznuje nijak pristup na service provider
        var envConfiguration = builder.Configuration
            .GetSection(Core.CisGlobalConstants.EnvironmentConfigurationSectionName)
            .Get<CisEnvironmentConfiguration>();

        builder
            .Services
            .AddOpenTelemetry()
            .ConfigureResource(res =>
            {
                res.AddService(envConfiguration!.DefaultApplicationKey!, envConfiguration.EnvironmentName);
                res.AddAttributes(new List<KeyValuePair<string, object>>
                {
                    new ("CisEnvironment", envConfiguration.EnvironmentName!)
                });
            })
            .WithTracing(tracing =>
            {
                tracing
                    .AddSqlClientInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddWcfInstrumentation()
                    .AddGrpcClientInstrumentation(options => options.SuppressDownstreamInstrumentation = true);

                // add OTLP
                if (!string.IsNullOrWhiteSpace(configuration.OTLP?.CollectorUrl))
                {
                    tracing.AddOtlpExporter(opts =>
                    {
                        opts.Endpoint = new Uri(configuration.OTLP.CollectorUrl!);
                    });
                }

                if (configuration.UseConsole)
                {
                    tracing.AddConsoleExporter();
                }
            })
            .WithMetrics(metrics =>
            {
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation();

                if (!string.IsNullOrWhiteSpace(configuration.OTLP?.CollectorUrl))
                {
                    metrics.AddOtlpExporter(opts =>
                    {
                        opts.Endpoint = new Uri(configuration.OTLP.CollectorUrl!);
                    });
                }
            });


        return builder;
    }

    internal const string _configurationTelemetryKey = "CisTelemetry:Tracing";
}

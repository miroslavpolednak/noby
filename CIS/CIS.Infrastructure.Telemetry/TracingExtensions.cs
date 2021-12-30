using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace CIS.Infrastructure.Telemetry;

public static class TracingExtensions
{
    /// <summary>
    /// Register Open Tracing instrumentation
    /// </summary>
    /// <param name="serviceName">Nazev sluzby, ktery se zobrazi v exporteru. Pokud neni zadano, hleda se v ICisEnvironmentConfiguration[DefaultApplicationKey]</param>
    /// <returns></returns>
    public static WebApplicationBuilder AddCisTracing(this WebApplicationBuilder builder, string? serviceName = null)
    {
        builder.Services.AddOpenTelemetryTracing(b =>
        {
            // set service name
            if (string.IsNullOrEmpty(serviceName))
            {
                var environmentConfiguration = b.GetServices().FirstOrDefault(t => t.ServiceType.Name == "ICisEnvironmentConfiguration")?.ImplementationInstance;
                if (environmentConfiguration is not null)
                    b.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(((Core.Configuration.ICisEnvironmentConfiguration)environmentConfiguration).DefaultApplicationKey));
            }
            else
                b.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName));

            // uses the default Jaeger settings
            b.AddJaegerExporter();

            // receive traces from built-in sources
            b.AddEntityFrameworkCoreInstrumentation();
            b.AddWcfInstrumentation();
            b.AddSqlClientInstrumentation();
            b.AddHttpClientInstrumentation();
            b.AddAspNetCoreInstrumentation();
            b.AddGrpcClientInstrumentation(options => options.SuppressDownstreamInstrumentation = true);
        });

        return builder;
    }
}

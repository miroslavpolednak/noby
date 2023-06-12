using CIS.Core.Configuration;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Filters;
using System.Reflection;

namespace CIS.Infrastructure.Telemetry;

internal sealed class LoggerBootstraper
{
    private readonly AssemblyName? _assemblyName;
    private readonly ICisEnvironmentConfiguration? _cisConfiguration;
    private readonly IConfiguration? _generalConfiguration;
    private readonly LogBehaviourTypes _logType;
    private readonly IServiceProvider _serviceProvider;

    static string[] _excludedGrpcRequestPaths = new[]
    {
        "/grpc.reflection.v1alpha.ServerReflection/ServerReflectionInfo",
        "/grpc.health.v1.Health/Check"
    };

    public LoggerBootstraper(HostBuilderContext hostingContext, IServiceProvider serviceProvider, LogBehaviourTypes logType)
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference. 
        _assemblyName = Assembly.GetEntryAssembly().GetName();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        _logType = logType;
        _generalConfiguration = hostingContext.Configuration;
        _serviceProvider = serviceProvider;
        _cisConfiguration = serviceProvider.GetRequiredService<ICisEnvironmentConfiguration>();
    }

    public void SetupFilters(LoggerConfiguration loggerConfiguration)
    {
        // global filter to exclude GRPC reflection
        //TODO any odstranit az se zbavime code-first grpc!
        if (_logType == LogBehaviourTypes.Grpc || _logType == LogBehaviourTypes.Any)
        {
            loggerConfiguration
                .Filter.ByExcluding(Matching.WithProperty<string>("RequestPath", t => _excludedGrpcRequestPaths.Contains(t)));
        }
        
        if (_logType == LogBehaviourTypes.WebApi)
        {
            // cokoliv jineho nez /api zahazovat
            loggerConfiguration
                .Filter.ByExcluding(Matching.WithProperty<string>("RequestPath", t => !t.StartsWith("/api/", StringComparison.OrdinalIgnoreCase)));
        }
        
        // remove health checks from logging
        loggerConfiguration
            .Filter.ByExcluding(Matching.WithProperty("RequestPath", CIS.Core.CisGlobalConstants.CisHealthCheckEndpointUrl));
    }

    public void EnrichLogger(LoggerConfiguration loggerConfiguration)
    {
        loggerConfiguration
            .ReadFrom.Configuration(_generalConfiguration!)
            .Enrich.With(_serviceProvider.GetRequiredService<Enrichers.NobyHeadersEnricher>())
            .Enrich.WithSpan()
            .Enrich.WithClientIp()
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithProperty("Assembly", $"{_assemblyName!.Name}")
            .Enrich.WithProperty("Version", $"{_assemblyName!.Version}");

        // enrich from CIS env
        if (_cisConfiguration is not null)
        {
            if (!string.IsNullOrEmpty(_cisConfiguration.EnvironmentName))
                loggerConfiguration.Enrich.WithProperty("CisEnvironment", _cisConfiguration.EnvironmentName);

            if (!string.IsNullOrEmpty(_cisConfiguration.DefaultApplicationKey))
                loggerConfiguration.Enrich.WithProperty("CisAppKey", _cisConfiguration.DefaultApplicationKey);
        }
    }

    public void AddOutputs(LoggerConfiguration loggerConfiguration, LogConfiguration configuration)
    {
        Helpers.AddOutputs(loggerConfiguration, configuration, _serviceProvider.GetService<TelemetryConfiguration>());
    }
}

using CIS.Core.Configuration;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Filters;
using System.Reflection;
using CIS.Core.Exceptions;
using Microsoft.Extensions.DependencyModel;
using Serilog.Settings.Configuration;

namespace CIS.Infrastructure.Telemetry;

internal sealed class LoggerBootstraper
{
    private readonly AssemblyName? _assemblyName;
    private readonly ICisEnvironmentConfiguration? _cisConfiguration;
    private readonly IConfiguration? _generalConfiguration;
    private readonly LoggingConfiguration _configuration;
    private readonly IServiceProvider _serviceProvider;

    static readonly string[] _excludedGrpcRequestPaths =
    [
        "/grpc.reflection.v1alpha.ServerReflection/ServerReflectionInfo",
        "/grpc.health.v1.Health/Check"
    ];

    public LoggerBootstraper(HostBuilderContext hostingContext, IServiceProvider serviceProvider, LoggingConfiguration configuration)
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference. 
        _assemblyName = Assembly.GetEntryAssembly().GetName();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        _configuration = configuration;
        _generalConfiguration = hostingContext.Configuration;
        _serviceProvider = serviceProvider;
        _cisConfiguration = serviceProvider.GetRequiredService<ICisEnvironmentConfiguration>();
    }

    public void SetupFilters(LoggerConfiguration loggerConfiguration)
    {
        // global filter to exclude GRPC reflection
        //TODO any odstranit az se zbavime code-first grpc!
        if (_configuration.LogType == LogBehaviourTypes.Grpc || _configuration.LogType == LogBehaviourTypes.Any)
        {
            loggerConfiguration
                .Filter
                .ByExcluding(Matching.WithProperty<string>("RequestPath", t => _excludedGrpcRequestPaths.Contains(t, StringComparer.OrdinalIgnoreCase)));
        }

#pragma warning disable CA1860 // Avoid using 'Enumerable.Any()' extension method
        if (_configuration.IncludeOnlyPaths?.Any() ?? false)
        {
            // cokoliv jineho nez /api zahazovat
            loggerConfiguration
                .Filter
                .ByExcluding(Matching.WithProperty<string>("RequestPath", t => !_configuration.IncludeOnlyPaths.Any(x => t.StartsWith(x, StringComparison.OrdinalIgnoreCase))));
        }
#pragma warning restore CA1860 // Avoid using 'Enumerable.Any()' extension method

        // remove health checks from logging
        loggerConfiguration
            .Filter
            .ByExcluding(Matching.WithProperty("RequestPath", CIS.Core.CisGlobalConstants.CisHealthCheckEndpointUrl));
        // divno HC
        loggerConfiguration
            .Filter
            .ByExcluding(Matching.WithProperty("RequestPath", "/health.html"));

        // vynechat vsechny chyby, ktere nechceme logovat
        loggerConfiguration
            .Filter
            .ByExcluding(logEvent => logEvent.Exception is ICisExceptionExludedFromLog);
    }

    public void EnrichLogger(LoggerConfiguration loggerConfiguration, LogConfiguration configuration)
    {
        loggerConfiguration
            .ReadFrom.Configuration(_generalConfiguration!, new ConfigurationReaderOptions(DependencyContext.Default))
            .Enrich.With(_serviceProvider.GetRequiredService<Enrichers.CisHeadersEnricher>())
            .Enrich.WithSpan()
            .Enrich.WithClientIp()
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithProperty("Assembly", $"{_assemblyName!.Name}")
            .Enrich.WithProperty("Version", $"{_assemblyName!.Version}");

        if (configuration.MaxPayloadLength.GetValueOrDefault() > 0)
        {
            loggerConfiguration.Enrich.With(new Enrichers.PayloadMaxLengthEnricher(configuration.MaxPayloadLength!.Value));
        }
        
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

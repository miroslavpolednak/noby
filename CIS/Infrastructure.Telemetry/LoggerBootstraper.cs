﻿using CIS.Core.Configuration;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Filters;
using Serilog.Sinks.MSSqlServer;
using System.Reflection;

namespace CIS.Infrastructure.Telemetry;

internal sealed class LoggerBootstraper
{
    private readonly AssemblyName? _assemblyName;
    private readonly ICisEnvironmentConfiguration? _cisConfiguration;
    private readonly IConfiguration? _generalConfiguration;
    private readonly LogBehaviourTypes _logType;
    private readonly IServiceProvider _serviceProvider;

    const string _fileLoggerTemplate = @"{Timestamp:yyyy-MM-dd HH:mm:ss,fff} [{ThreadId}] {Level:u} - [{TraceId}] [] [{Assembly}] [{Version}] [{MachineName}] [{CisUserId}] [{RequestPath}] - {Message}{NewLine}";
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
            .ReadFrom.Configuration(_generalConfiguration)
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

#pragma warning disable CA1822 // Mark members as static
    public void AddOutputs(LoggerConfiguration loggerConfiguration, LogConfiguration configuration)
#pragma warning restore CA1822 // Mark members as static
    {
        // app insights
        if (configuration.ApplicationInsights is not null)
        {
            loggerConfiguration
                .WriteTo
                .ApplicationInsights(_serviceProvider.GetRequiredService<TelemetryConfiguration>(), TelemetryConverter.Traces);
        }

        // seq
        if (!string.IsNullOrEmpty(configuration.Seq?.ServerUrl))
        {
            loggerConfiguration
                .WriteTo
                .Seq(configuration.Seq.ServerUrl, eventBodyLimitBytes: 1048576);
        }

        // logovani do souboru
        if (configuration.File is not null)
        {
            var path = Path.Combine(configuration.File.Path, configuration.File.Filename);

#pragma warning disable CA1305 // Specify IFormatProvider
            loggerConfiguration
                .WriteTo
                .Async(a => a.File(path, buffered: true, rollingInterval: RollingInterval.Day, outputTemplate: _fileLoggerTemplate), bufferSize: 1000);
#pragma warning restore CA1305 // Specify IFormatProvider
        }

        // logovani do databaze
        //TODO tohle poradne dodelat nebo uplne vyhodit - moc se mi do DB logovat nechce, ale jestli nebude nic jinyho nez Logman, tak asi nutnost
        if (!string.IsNullOrEmpty(configuration.Database?.ConnectionString))
        {
            MSSqlServerSinkOptions sqlOptions = new()
            {
                AutoCreateSqlTable = true,
                SchemaName = "dbo",
                TableName = "CisLog"
            };
            ColumnOptions sqlColumns = new();

#pragma warning disable CA1305 // Specify IFormatProvider
            loggerConfiguration
                .WriteTo
                .MSSqlServer(
                    connectionString: configuration.Database.ConnectionString,
                    sinkOptions: sqlOptions,
                    columnOptions: sqlColumns
                );
#pragma warning restore CA1305 // Specify IFormatProvider
        }
        
        // console output
        if (configuration.UseConsole)
        {
#pragma warning disable CA1305 // Specify IFormatProvider
            loggerConfiguration
                .WriteTo
                .Console();
#pragma warning restore CA1305 // Specify IFormatProvider
        }
    }
}

using CIS.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Sinks.MSSqlServer;
using System.Reflection;

namespace CIS.Infrastructure.Telemetry;

internal class LoggerBootstraper
{
    private readonly AssemblyName? _assemblyName;
    private readonly ICisEnvironmentConfiguration? _cisConfiguration;
    private readonly IConfiguration? _generalConfiguration;

    public LoggerBootstraper(HostBuilderContext hostingContext, IServiceProvider serviceProvider)
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference. 
        _assemblyName = Assembly.GetEntryAssembly().GetName();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        _generalConfiguration = hostingContext.Configuration;

        _cisConfiguration = serviceProvider.GetRequiredService<ICisEnvironmentConfiguration>();
    }

    public void EnrichLogger(LoggerConfiguration loggerConfiguration)
    {
        loggerConfiguration
            .ReadFrom.Configuration(_generalConfiguration)
            .Enrich.WithSpan()
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithProperty("Assembly", $"{_assemblyName!.Name}")
            .Enrich.WithProperty("Version", $"{_assemblyName!.Version}")
            .Enrich.WithProperty("ThreadId", $"{Environment.CurrentManagedThreadId}");

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
        // seq
        if (configuration.Seq is not null)
        {
            loggerConfiguration
                .WriteTo
                .Seq(configuration.Seq.ServerUrl);
        }

        // logovani do souboru
        if (configuration.File is not null)
        {
            var path = Path.Combine(configuration.File.Path, configuration.File.Filename);
            var template = @"{Timestamp: yyyy-MM-dd HH:mm:ss,fff} [{ThreadId}] {Level:u} - [{TraceId}] [] [{Assembly}] [{Version}] [{MachineName}] [{CisUserId}] [{RequestPath}] - {Message}{NewLine}";

            loggerConfiguration
                .WriteTo
                .Async(a => a.File(path, buffered: true, rollingInterval: RollingInterval.Day, outputTemplate: template), bufferSize: 1000);
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

            loggerConfiguration
                .WriteTo
                .MSSqlServer(
                    connectionString: configuration.Database.ConnectionString,
                    sinkOptions: sqlOptions,
                    columnOptions: sqlColumns
                );
        }
        
        // console output
        if (configuration.UseConsole)
        {
            loggerConfiguration
                .WriteTo
                .Console();
        }
    }
}

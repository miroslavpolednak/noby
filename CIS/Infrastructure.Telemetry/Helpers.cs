using Microsoft.ApplicationInsights.Extensibility;
using Serilog.Sinks.MSSqlServer;
using Serilog;

namespace CIS.Infrastructure.Telemetry;

internal static class Helpers
{
    const string _fileLoggerTemplate = @"{Timestamp:yyyy-MM-dd HH:mm:ss,fff} [{ThreadId}] [{Level:u}] [{TraceId}] [{SpanId}] [{ParentId}] [{CisAppKey}] [{Version}] [{Assembly}] [{SourceContext}] [{MachineName}] [{ClientIp}] [{CisUserId}] [{CisUserIdent}] [{RequestId}] [{RequestPath}] [{ConnectionId}] [{Message}] [{Exception}]{NewLine}";

    public static void AddOutputs(
        LoggerConfiguration loggerConfiguration, 
        LogConfiguration configuration,
        TelemetryConfiguration? telemetryConfiguration)
#pragma warning restore CA1822 // Mark members as static
    {
        // app insights
        if (configuration.ApplicationInsights is not null && telemetryConfiguration is not null)
        {
            loggerConfiguration
                .WriteTo
                .ApplicationInsights(telemetryConfiguration, TelemetryConverter.Traces);
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

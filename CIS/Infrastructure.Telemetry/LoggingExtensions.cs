using CIS.Core.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Sinks.MSSqlServer;
using System.Reflection;

namespace CIS.Infrastructure.Telemetry;

public static class LoggingExtensions
{
    public static IApplicationBuilder UseCisLogging(this IApplicationBuilder webApplication)
    {
        webApplication.UseMiddleware<Serilog.SerilogCisUserMiddleware>();
        return webApplication;
    }

    public static WebApplicationBuilder AddCisLogging(this WebApplicationBuilder builder)
    {
        builder.Host.AddCisLogging();
        return builder;
    }

    public static IHostBuilder AddCisLogging(this IHostBuilder builder)
    {
        builder.UseSerilog((hostingContext, serviceProvider, loggerConfiguration) =>
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference. 
            var assembly = Assembly.GetEntryAssembly().GetName();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            // get configuration from json file
            var configSection = hostingContext.Configuration.GetSection("CisTelemetry");
            CisTelemetryConfiguration configuration = new();
            configSection.Bind(configuration);

            loggerConfiguration
                .ReadFrom.Configuration(hostingContext.Configuration)
                .Enrich.WithSpan()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Assembly", $"{assembly.Name}")
                .Enrich.WithProperty("Version", $"{assembly.Version}");

            // enrich from CIS env
            var cisEnvConfiguration = serviceProvider.GetService(typeof(ICisEnvironmentConfiguration)) as ICisEnvironmentConfiguration;
            if (cisEnvConfiguration is not null)
            {
                loggerConfiguration
                    .Enrich.WithProperty("CisEnvironment", cisEnvConfiguration.EnvironmentName)
                    .Enrich.WithProperty("CisAppKey", cisEnvConfiguration.DefaultApplicationKey);
            }

            // seq
            if (configuration.Logging?.Seq is not null)
            {
                loggerConfiguration.WriteTo.Seq(configuration.Logging.Seq.ServerUrl);
            }

            // logovani do souboru
            if (configuration.Logging?.File is not null)
            {
                loggerConfiguration
                    .WriteTo
                    .Async(a =>
                        a.File(Path.Combine(configuration.Logging.File.Path, configuration.Logging.File.Filename), buffered: true, rollingInterval: RollingInterval.Day),
                    bufferSize: 1000);
            }

            // logovani do databaze
            //TODO tohle poradne dodelat nebo uplne vyhodit - moc se mi do DB logovat nechce, ale jestli nebude nic jinyho nez Logman, tak asi nutnost
            if (configuration.Logging?.Database is not null)
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
                        connectionString: configuration.Logging.Database.ConnectionString,
                        sinkOptions: sqlOptions,
                        columnOptions: sqlColumns
                    );
            }

            // console output
            if (configuration.Logging?.UseConsole ?? false)
            {
                loggerConfiguration
                    .WriteTo.Console();
            }
        });

        return builder;
    }
}

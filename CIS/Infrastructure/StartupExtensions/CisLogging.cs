using CIS.Infrastructure.Configuration;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Reflection;
using System.Linq;

namespace CIS.Infrastructure.StartupExtensions;

public static class CisLogging
{
    public static IHostBuilder AddCisLogging(this IHostBuilder builder)
    {
        builder.UseSerilog((hostingContext, serviceProvider, loggerConfiguration) =>
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference. 
            var assembly = Assembly.GetEntryAssembly().GetName();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            loggerConfiguration
                .ReadFrom.Configuration(hostingContext.Configuration)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Assembly", $"{assembly.Name}")
                .Enrich.WithProperty("Version", $"{assembly.Version}");

            // get configuration from json file
            var configSection = hostingContext.Configuration.GetSection("CisLogging");
            CisLoggingConfiguration configuration = new();
            configSection.Bind(configuration);
            
            // logovani do souboru
            if (configuration.File is not null)
            {
                loggerConfiguration
                    .WriteTo
                    .Async(a => 
                        a.File(Path.Combine(configuration.File.Path, configuration.File.Filename), buffered: true, rollingInterval: RollingInterval.Day), 
                    bufferSize: 1000);
            }

            // logovani do databaze
            if (configuration.Database is not null)
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
            loggerConfiguration
                .WriteTo.Console();

        });
        return builder;
    }

    public static void CloseAndFlushLog()
    {
        Log.CloseAndFlush();
    }
}

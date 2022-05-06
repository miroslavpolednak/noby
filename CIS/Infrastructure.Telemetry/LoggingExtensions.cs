using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog.Filters;
using Serilog;

namespace CIS.Infrastructure.Telemetry;

public static class LoggingExtensions
{
    public static IApplicationBuilder UseCisLogging(this IApplicationBuilder webApplication)
    {
        webApplication.UseMiddleware<Middlewares.LoggerCisUserMiddleware>();

        webApplication.UseSerilogRequestLogging();

        return webApplication;
    }

    public static WebApplicationBuilder AddCisLogging(this WebApplicationBuilder builder)
    {
        // auditni log
        builder.Services.AddSingleton<IAuditLogger>(new AuditLogger());

        // pridani serilogu
        builder.Host.AddCisLogging();

        // pridani request behaviour mediatru - loguje request a response objekty
        builder.Services.AddTransient(typeof(MediatR.IPipelineBehavior<,>), typeof(Mediatr.LoggingBehaviour<,>));

        return builder;
    }

    /// <summary>
    /// Pri ukonceni aplikaci se ujisti, ze vsechny sinky jsou vyprazdnene
    /// </summary>
    public static void CloseAndFlush()
    {
        Log.CloseAndFlush();
        AuditLogger.CloseAndFlush();

        // kdyz tu neni sleep, tak se obcas nezapsal vsechen output pri ukonceni sluzby
        Thread.Sleep(2000);
    }

    private static IHostBuilder AddCisLogging(this IHostBuilder builder)
    {
        builder.UseSerilog((hostingContext, serviceProvider, loggerConfiguration) =>
        {
            // get configuration from json file
            var configSection = hostingContext.Configuration.GetSection(_configurationTelemetryKey);
            CisTelemetryConfiguration _configuration = new();
            configSection.Bind(_configuration);

            if (_configuration.Logging is not null)
            {
                var bootstrapper = new LoggerBootstraper(hostingContext, serviceProvider);

                // global filter to exclude GRPC reflection
                if (_configuration.Logging.LogType == LogBehaviourTypes.Grpc)
                    loggerConfiguration
                        .Filter.ByExcluding(Matching.WithProperty("RequestPath", "/grpc.reflection.v1alpha.ServerReflection/ServerReflectionInfo"));

                // general log setup
                if (_configuration?.Logging?.Application is not null)
                {
                    bootstrapper.EnrichLogger(loggerConfiguration);
                    bootstrapper.AddOutputs(loggerConfiguration, _configuration.Logging.Application);
                }

                // audit log setup
                if (_configuration?.Logging?.Audit is not null)
                {
                    AuditLogger.SetupLogger(bootstrapper, _configuration.Logging.Audit);
                }
            }
        });

        return builder;
    }

    const string _configurationTelemetryKey = "CisTelemetry";
}

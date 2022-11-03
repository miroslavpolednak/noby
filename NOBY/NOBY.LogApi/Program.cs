using CIS.Infrastructure.StartupExtensions;
using CIS.Infrastructure.Telemetry;
using Microsoft.AspNetCore.Mvc;

namespace NOBY.LogApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder
            .AddCisEnvironmentConfiguration()
            .AddCisWebApiCors()
            .AddCisLogging()
            .AddCisHealthChecks();

        var app = builder.Build();

        // logovani standardniho logu
        app.MapPost("/log", (LogModel model, [FromServices] ILogger<Program> logger) =>
        {
#pragma warning disable CA2254 // Template should be a static expression
#pragma warning disable CA1848 // Use the LoggerMessage delegates
            logger.Log(model.Level.ToLogLevel(), model.Message);
#pragma warning restore CA1848 // Use the LoggerMessage delegates
#pragma warning restore CA2254 // Template should be a static expression
        });

        // logovani auditniho logu
        app.MapPost("/audit", (LogModel model, [FromServices] IAuditLogger logger) =>
        {
#pragma warning disable CA2254 // Template should be a static expression
#pragma warning disable CA1848 // Use the LoggerMessage delegates
            logger.Log(model.Message ?? "");
#pragma warning restore CA1848 // Use the LoggerMessage delegates
#pragma warning restore CA2254 // Template should be a static expression
        });

        try
        {
            app.Run();
        }
        finally
        {
            LoggingExtensions.CloseAndFlush();
        }
    }
}
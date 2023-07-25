using CIS.Infrastructure.Telemetry;
using Microsoft.AspNetCore.Mvc;

namespace NOBY.LogApi;

internal static class Endpoints
{
    public static WebApplication RegisterLoggerEndpoints(this WebApplication app)
    {
        // logovani standardniho logu
        app.MapPost("/log", (LogModel model, [FromServices] ILogger<Program> logger) =>
        {
#pragma warning disable CA2254 // Template should be a static expression
#pragma warning disable CA1848 // Use the LoggerMessage delegates
            logger.Log(model.Level.ToLogLevel(), model.Message);
#pragma warning restore CA1848 // Use the LoggerMessage delegates
#pragma warning restore CA2254 // Template should be a static expression
        })
            .RequireCors("_cors")
            .WithDescription("Logování do standardního aplikačního logu.")
            .WithTags("Logging")
            .WithOpenApi();

        // logovani auditniho logu
        app.MapPost("/audit", (AuditLogModel model, [FromServices] IAuditLogger logger) =>
        {
            //logger.Log(model.Message ?? "");
        })
            .RequireCors("_cors")
            .WithDescription("Logování do auditního logu.")
            .WithTags("Logging")
            .WithOpenApi();

        return app;
    }
}

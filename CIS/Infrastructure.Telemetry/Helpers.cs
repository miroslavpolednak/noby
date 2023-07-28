using Microsoft.ApplicationInsights.Extensibility;
using Serilog.Sinks.MSSqlServer;
using Serilog;
using CIS.Core.Security;
using Microsoft.AspNetCore.Http;

namespace CIS.Infrastructure.Telemetry;

public static class Helpers
{
    const string _fileLoggerTemplate = @"{Timestamp:yyyy-MM-dd HH:mm:ss,fff} [{ThreadId}] [{Level:u}] [{TraceId}] [{SpanId}] [{ParentId}] [{CisAppKey}] [{Version}] [{Assembly}] [{SourceContext}] [{MachineName}] [{ClientIp}] [{CisUserId}] [{CisUserIdent}] [{RequestId}] [{RequestPath}] [{ConnectionId}] [{Message}] [{Exception}]{NewLine}";

    public static (int? UserId, string? UserIdent) GetCurrentUser(ICurrentUserAccessor? userAccessor, IHttpContextAccessor httpContextAccessor)
    {
        // mam v kontextu instanci uzivatele
        if (userAccessor is not null && userAccessor.IsAuthenticated)
        {
            return (userAccessor.User!.Id, userAccessor.User!.Login);
        }
        // neni instance uzivatele, zkus se kouknout do http hlavicek
        else if (hasKey(SecurityConstants.ContextUserHttpHeaderUserIdKey))
        {
            int? userId = null;
            string? userIdent = null;

            if (int.TryParse(httpContextAccessor.HttpContext!.Request.Headers[SecurityConstants.ContextUserHttpHeaderUserIdKey].First(), out int u))
            {
                userId = u;
            }

            if (hasKey(SecurityConstants.ContextUserHttpHeaderUserIdentKey))
            {
                userIdent = httpContextAccessor.HttpContext!.Request.Headers[SecurityConstants.ContextUserHttpHeaderUserIdentKey].First();
            }

            return (userId, userIdent);
        }
        // posledni pokus - muze byt jiz vytvorena claims identity, ale jeste neni v kontextu User z auth middlewaru
        else if (httpContextAccessor.HttpContext?.User?.HasClaim(t => t.Type == SecurityConstants.ClaimTypeIdent) ?? false)
        {
            return (null, httpContextAccessor.HttpContext!.User.Claims.First(t => t.Type == SecurityConstants.ContextUserHttpHeaderUserIdentKey).Value);
        }

        return (null, null);

        bool hasKey(string key)
        {
            return httpContextAccessor.HttpContext?.Request?.Headers?.ContainsKey(key) ?? false;
        }
    }

    internal static void AddOutputs(
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

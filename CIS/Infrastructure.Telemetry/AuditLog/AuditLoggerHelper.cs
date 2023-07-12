using CIS.Core.Configuration;
using Serilog.Formatting.Json;
using System.Globalization;

namespace CIS.Infrastructure.Telemetry.AuditLog;

internal sealed class AuditLoggerHelper
{
    private readonly JsonValueFormatter _valueFormatter;
    private readonly Database.DatabaseWriter _databaseWriter;
    private AuditLoggerDefaults _loggerDefaults;

    public AuditLoggerHelper(string serverIp, ICisEnvironmentConfiguration environmentConfiguration, AuditLogConfiguration auditConfiguration)
    {
        _valueFormatter = new JsonValueFormatter(typeTagName: "$type");
        _databaseWriter = new Database.DatabaseWriter(auditConfiguration.ConnectionString);
        _loggerDefaults = new AuditLoggerDefaults(serverIp, environmentConfiguration.DefaultApplicationKey!, auditConfiguration.EamApplication, environmentConfiguration.EnvironmentName!);
    }

    public void Log(AuditEventTypes eventType, ref AuditEventHeaders headers)
    {
        StringWriter json = new();
        AuditLoggerJsonWriter.CreateJson(json, _valueFormatter, ref _loggerDefaults, ref headers);

        var eventObject = new Database.AuditEvent((int)eventType, json.ToString());
        _databaseWriter.Write(ref eventObject);
    }
}

internal static class AuditLoggerJsonWriter
{
    private static CultureInfo _culture = CultureInfo.InvariantCulture;

    public static void CreateJson(
        StringWriter output, 
        JsonValueFormatter valueFormatter, 
        ref AuditLoggerDefaults loggerDefaults, 
        ref AuditEventHeaders headers)
    {
        var time = DateTime.Now.ToString("o", _culture).AsSpan();

        output.Write("{\"id\":\"");
        output.Write(Guid.NewGuid().ToString());
        output.Write("\",");

        #region meta
        output.Write("\"@meta\":{");

        // logger
        output.Write("\"logger\":");
        write(output, loggerDefaults.CisAppKey.AsSpan());

        // timestamp
        output.Write(",\"timestamp\":");
        write(output, time);

        // version

        // eam
        output.Write(",\"eamApplication\":");
        write(output, loggerDefaults.EamApplication.AsSpan());

        output.Write("},");
        #endregion meta

        #region header
        output.Write("\"header\":{");

        #region event
        output.Write("\"event\":{");

        // correlation
        output.Write("\"correlation\":");
        write(output, "");

        // time
        output.Write(",\"time\":{\"tsServer\":");
        write(output, time);
        output.Write("}");

        // type
        output.Write(",\"type\":{");
        output.Write("\"id\":");
        output.Write(1);
        output.Write(",\"version\":");
        output.Write(1);
        output.Write("},");

        // source
        output.Write(",\"source\":{");
        output.Write("\"id\":");
        write(output, loggerDefaults.CisAppKey.AsSpan());
        output.Write("\"name\":");
        write(output, loggerDefaults.EamApplication.AsSpan());
        output.Write("\"environment\":");
        write(output, loggerDefaults.EnvironmentName.AsSpan());
        output.Write("\"ipClient\":");
        
        output.Write("\"ipServer\":");
        write(output, loggerDefaults.ServerIp.AsSpan());
        output.Write("},");

        output.Write("},");
        #endregion event

        output.Write("},");
        #endregion header

        output.Write("}");
    }

    private static void write(StringWriter output, ReadOnlySpan<char> text)
    {
        output.Write("\"");
        output.Write(text);
        output.Write("\"");
    }
}

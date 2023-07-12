using CIS.Core.Configuration;
using Serilog.Formatting.Json;
using System.Globalization;
using System.ServiceModel.Description;

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

    public void Log(AuditEventContext context)
    {
        StringWriter json = new();
        AuditLoggerJsonWriter.CreateJson(json, _valueFormatter, ref _loggerDefaults, context);

        var eventObject = new Database.AuditEvent((int)context.EventType, json.ToString());
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
        AuditEventContext context)
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
        output.Write(",\"version\":");
        output.Write(3);

        // eam
        output.Write(",\"eamApplication\":");
        write(output, loggerDefaults.EamApplication.AsSpan());

        output.Write("}");
        #endregion meta

        #region header
        output.Write(",\"header\":{");

        #region event
        output.Write("\"event\":{");

        // correlation
        output.Write("\"correlation\":");
        write(output, context.Correlation);

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
        output.Write("}");

        // source
        output.Write(",\"source\":{");
        output.Write("\"id\":");
        write(output, loggerDefaults.CisAppKey.AsSpan());
        output.Write(",\"name\":");
        write(output, loggerDefaults.EamApplication.AsSpan());
        output.Write(",\"environment\":");
        write(output, loggerDefaults.EnvironmentName.AsSpan());
        output.Write(",\"ipClient\":");
        write(output, context.ClientIp.AsSpan());
        output.Write(",\"ipServer\":");
        write(output, loggerDefaults.ServerIp.AsSpan());
        output.Write("}");

        output.Write("}");
        #endregion event

        #region actor
        if (!string.IsNullOrEmpty(context.UserIdent))
        {
            int idx = context.UserIdent.IndexOf('=', 1);
            output.Write(",\"actor\":{");
            output.Write("\"idSchema\":");
            write(output, context.UserIdent[..idx].AsSpan());
            output.Write(",\"id\":");
            write(output, context.UserIdent[(idx+1)..].AsSpan());
            output.Write("}");
        }
        #endregion actor

        if (context.Identities is not null && context.Identities.Any())
        {
            output.Write(",\"identity\":[");
            writeHeaderCollection(output, context.Identities);
            output.Write("]");
        }

        if (context.Products is not null && context.Products.Any())
        {
            output.Write(",\"product\":[");
            writeHeaderCollection(output, context.Products);
            output.Write("]");
        }

        if (context.Operation is not null)
        {
            output.Write(",\"operation\":{\"id\":");
            write(output, context.Operation.Id);
            output.Write(",\"idSchema\":");
            write(output, context.Operation.Type);
            output.Write("}");
        }

        output.Write("}");
        #endregion header

        output.Write("}");
    }

    private static void writeHeaderCollection(StringWriter output, ICollection<AuditLoggerHeaderItem> items)
    {
        int i = 0;
        foreach (var identity in items)
        {
            if (i++ > 0) output.Write(",");
            output.Write("{\"id\":");
            write(output, identity.Id);
            output.Write(",\"idSchema\":");
            write(output, identity.Type);
            output.Write("}");
        }
    }

    private static void write(StringWriter output, ReadOnlySpan<char> text)
    {
        output.Write("\"");
        output.Write(text);
        output.Write("\"");
    }
}

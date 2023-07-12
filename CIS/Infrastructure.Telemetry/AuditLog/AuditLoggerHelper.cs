using CIS.Core;
using CIS.Core.Configuration;
using FastEnumUtility;
using System.Globalization;

namespace CIS.Infrastructure.Telemetry.AuditLog;

internal sealed class AuditLoggerHelper
{
    private readonly Database.DatabaseWriter _databaseWriter;
    private AuditLoggerDefaults _loggerDefaults;
    static Dictionary<AuditEventTypes, AuditEventTypeDescriptorAttribute> _eventTypeDescriptors = new();

    // cache event types
    static AuditLoggerHelper()
    {
        var values = FastEnum.GetValues<AuditEventTypes>();
        foreach (var v in values)
        {
            _eventTypeDescriptors.Add(v, v.GetAttribute<AuditEventTypeDescriptorAttribute>()!);
        }
    }

    public AuditLoggerHelper(string serverIp, ICisEnvironmentConfiguration environmentConfiguration, AuditLogConfiguration auditConfiguration)
    {
        _databaseWriter = new Database.DatabaseWriter(auditConfiguration.ConnectionString);
        _loggerDefaults = new AuditLoggerDefaults(serverIp, environmentConfiguration.DefaultApplicationKey!, auditConfiguration.EamApplication, auditConfiguration.EamVersion, environmentConfiguration.EnvironmentName!);
    }

    public void Log(AuditEventContext context)
    {
        var eventDescriptor = _eventTypeDescriptors[context.EventType];
        StringWriter json = new();
        AuditLoggerJsonWriter.CreateJson(json, ref _loggerDefaults, context, eventDescriptor.Code.AsSpan(), eventDescriptor.Version);

        var eventObject = new Database.AuditEvent(context.AuditEventIdent, eventDescriptor.Code, json.ToString());
        _databaseWriter.Write(ref eventObject);
    }
}

internal static class AuditLoggerJsonWriter
{
    private static CultureInfo _culture = CultureInfo.InvariantCulture;

    public static void CreateJson(
        StringWriter output,
        ref AuditLoggerDefaults loggerDefaults, 
        AuditEventContext context,
        ReadOnlySpan<char> eventTypeId,
        int eventTypeVersion)
    {
        var time = DateTime.Now.ToString("o", _culture).AsSpan();

        output.Write("{\"id\":\"");
        output.Write(context.AuditEventIdent.ToString());
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
        output.Write(loggerDefaults.EamVersion.AsSpan());

        // eam
        output.Write(",\"eamApplication\":");
        write(output, loggerDefaults.EamApplication.AsSpan());

        output.Write("}");
        #endregion meta

        #region header
        output.Write(",\"header\":{");

        output.Write("\"message\":");
        write(output, context.Message.AsSpan());

        #region event
        output.Write(",\"event\":{");

        // correlation
        output.Write("\"correlation\":");
        write(output, context.Correlation.AsSpan());

        // time
        output.Write(",\"time\":{\"tsServer\":");
        write(output, time);
        output.Write("}");

        // type
        output.Write(",\"type\":{");
        output.Write("\"id\":");
        write(output, eventTypeId);
        output.Write(",\"version\":");
        output.Write(eventTypeVersion);
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
            writeHeaderIdentity(output, context.Identities);
            output.Write("]");
        }

        if (context.Products is not null && context.Products.Any())
        {
            output.Write(",\"product\":[");
            writeHeaderProduct(output, context.Products);
            output.Write("]");
        }

        if (context.Operation is not null)
        {
            output.Write(",\"operation\":{\"type\":");
            write(output, context.Operation.Type.AsSpan());
            if (!string.IsNullOrEmpty(context.Operation.Id))
            {
                output.Write(",\"id\":");
                write(output, context.Operation.Id.AsSpan());
            }
            output.Write("}");
        }

        output.Write("}");
        #endregion header

        #region body
        output.Write(",\"body\":{");

        if (context.BodyBefore is not null)
        {
            output.Write("\"objectsBefore\":{");
            writeBodyCollection(output, context.BodyBefore);
            output.Write("}");
        }

        output.Write("}");
        #endregion body

        output.Write("}");
    }

    private static void writeBodyCollection(StringWriter output, IDictionary<string, string> items)
    {
        int i = 0;
        foreach (var item in items)
        {
            if (i++ > 0) output.Write(",");
            output.Write("\"");
            output.Write(item.Key.AsSpan());
            output.Write("\":\"");
            output.Write(item.Value.AsSpan());
            output.Write("\"");
        }
    }

    private static void writeHeaderIdentity(StringWriter output, ICollection<AuditLoggerHeaderItem> items)
    {
        int i = 0;
        foreach (var identity in items)
        {
            if (i++ > 0) output.Write(",");
            output.Write("{\"id\":");
            write(output, identity.Id.AsSpan());
            output.Write(",\"idSchema\":");
            write(output, identity.Type.AsSpan());
            output.Write("}");
        }
    }

    private static void writeHeaderProduct(StringWriter output, ICollection<AuditLoggerHeaderItem> items)
    {
        int i = 0;
        foreach (var product in items)
        {
            if (i++ > 0) output.Write(",");
            output.Write("{\"type\":");
            write(output, product.Type.AsSpan());
            if (!string.IsNullOrEmpty(product.Id))
            {
                output.Write(",\"id\":");
                write(output, product.Id.AsSpan());
            }
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

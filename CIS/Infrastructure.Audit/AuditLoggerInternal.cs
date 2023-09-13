using CIS.Core;
using CIS.Core.Configuration;
using CIS.Infrastructure.Audit.Attributes;
using CIS.Infrastructure.Audit.Configuration;
using CIS.Infrastructure.Audit.Dto;
using FastEnumUtility;
using System.Globalization;
using System.Text;

namespace CIS.Infrastructure.Audit;

internal sealed class AuditLoggerInternal
{
    private readonly Database.DatabaseWriter _databaseWriter;
    private AuditLoggerDefaults _loggerDefaults;
    private readonly byte[] _hashSecretKey;
    
    static Dictionary<AuditEventTypes, AuditEventTypeDescriptorAttribute> _eventTypeDescriptors = new();
    private static CultureInfo _culture = CultureInfo.InvariantCulture;

    // cache event types
    static AuditLoggerInternal()
    {
        var values = FastEnum.GetValues<AuditEventTypes>();
        foreach (var v in values)
        {
            _eventTypeDescriptors.Add(v, v.GetAttribute<AuditEventTypeDescriptorAttribute>()!);
        }
    }

    public AuditLoggerInternal(
        string serverIp, 
        ICisEnvironmentConfiguration environmentConfiguration, 
        AuditLogConfiguration auditConfiguration)
    {
        if (auditConfiguration is null)
        {
            throw new ArgumentNullException(nameof(auditConfiguration));
        }

        _databaseWriter = new Database.DatabaseWriter(auditConfiguration.ConnectionString);
        _loggerDefaults = new AuditLoggerDefaults(serverIp, environmentConfiguration.DefaultApplicationKey!, auditConfiguration.EamApplication, auditConfiguration.EamVersion, environmentConfiguration.EnvironmentName!);
        _hashSecretKey = Encoding.UTF8.GetBytes(auditConfiguration.HashSecretKey);
    }

    public void Log(AuditEventContext context)
    {
        var eventDescriptor = _eventTypeDescriptors[context.EventType];

        // check event result
        if (eventDescriptor.Results is null && !string.IsNullOrEmpty(context.Result))
        {
            throw new InvalidOperationException($"Audit log event of type '{context.EventType}' can not contain result");
        }
        else if (eventDescriptor.Results is not null 
            && !string.IsNullOrEmpty(context.Result)
            && !eventDescriptor.Results.Contains(context.Result))
        {
            throw new InvalidOperationException($"Audit log result '{context.Result}' is not valid for event of type '{context.EventType}'");
        }

        // next seq no
        long? seqId = eventDescriptor.GenerateSequenceNumber ? _databaseWriter.GetSequenceId() : default(long?);
        string? hashId = null;
        var time = DateTime.Now.ToString("o", _culture);

        // vytvorit json je proto, aby se spocital hash
        StringWriter jsonBeforeHash = new();
        AuditLoggerJsonWriter.CreateJson(jsonBeforeHash, ref time, ref hashId, ref seqId, ref _loggerDefaults, context, eventDescriptor.Code.AsSpan(), eventDescriptor.Version);
        hashId = HashHelper.HashMessage(jsonBeforeHash.ToString(), _hashSecretKey);

        // vytvorit json znova s hashem
        // seru na to ze je to takhle pomale, kdyz to soudruzi chteji...
        StringWriter jsonAfterHash = new();
        AuditLoggerJsonWriter.CreateJson(jsonAfterHash, ref time, ref hashId, ref seqId, ref _loggerDefaults, context, eventDescriptor.Code.AsSpan(), eventDescriptor.Version);

        var eventObject = new Database.AuditEvent(hashId!, eventDescriptor.Code, jsonAfterHash.ToString());
        _databaseWriter.Write(ref eventObject);
    }
}

internal static class AuditLoggerJsonWriter
{
    public static void CreateJson(
        StringWriter output,
        ref string time,
        ref string? hashId,
        ref long? sequenceId,
        ref AuditLoggerDefaults loggerDefaults, 
        AuditEventContext context,
        ReadOnlySpan<char> eventTypeId,
        int eventTypeVersion)
    {
        output.Write("{\"id\":\"");
        if (!string.IsNullOrEmpty(hashId))
            output.Write(hashId);
        output.Write("\",");

        #region meta
        output.Write("\"@meta\":{");

        // logger
        output.Write("\"logger\":\"NOBY\"");

        // seqno
        if (sequenceId.HasValue)
        {
            output.Write(",\"seqNo\":");
            write(output, sequenceId.Value.ToString(CultureInfo.InvariantCulture));
        }

        // timestamp
        output.Write(",\"timestamp\":");
        write(output, time);

        // version
        output.Write(",\"version\":");
        write(output, loggerDefaults.EamVersion.AsSpan());

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

        // result
        output.Write(",\"result\":");
        write(output, context.Result.AsSpan());

        // time
        output.Write(",\"time\":{\"tsServer\":");
        write(output, time);
        output.Write("}");

        // type
        output.Write(",\"type\":{");
        output.Write("\"id\":");
        write(output, eventTypeId);
        output.Write(",\"version\":");
        write(output, eventTypeVersion.ToString(CultureInfo.InvariantCulture));
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

        if (context.BodyAfter is not null)
        {
            if (context.BodyBefore is not null) output.Write(",");

            output.Write("\"objectsAfter\":{");
            writeBodyCollection(output, context.BodyAfter);
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

using CIS.Core;
using CIS.Core.Configuration;
using SharedAudit.Attributes;
using SharedAudit.Configuration;
using SharedAudit.Dto;
using FastEnumUtility;
using System.Globalization;
using System.Text;

namespace SharedAudit;

internal sealed class AuditLoggerInternal 
    : IAuditLoggerInternal
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
        ArgumentNullException.ThrowIfNull(auditConfiguration);

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
        long? seqId = eventDescriptor.GenerateSequenceNumber ? context.SequenceId ?? _databaseWriter.GetSequenceId() : default(long?);
        string? hashId = null;
        var time = context.Timestamp.ToString("o", _culture);

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

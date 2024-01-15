namespace SharedAudit;

internal sealed class ManualAuditLogger
    : IManualAuditLogger
{
    public void Log(
        AuditEventTypes eventType,
        string message,
        DateTime timestamp,
        string correlation,
        string? ipAddress,
        string? userIdent,
        long? sequenceId,
        ICollection<AuditLoggerHeaderItem>? identities = null,
        ICollection<AuditLoggerHeaderItem>? products = null,
        AuditLoggerHeaderItem? operation = null,
        string? result = null,
        IDictionary<string, string>? bodyBefore = null,
        IDictionary<string, string>? bodyAfter = null)
    {
        var context = new Dto.AuditEventContext()
        {
            EventType = eventType,
            Message = message,
            Result = result,
            Identities = identities,
            Products = products,
            Operation = operation,
            BodyAfter = bodyAfter,
            BodyBefore = bodyBefore,
            ClientIp = ipAddress ?? "",
            UserIdent = userIdent,
            Timestamp = timestamp,
            Correlation = correlation,
            SequenceId = sequenceId
        };
        _helper.Log(context);
    }

    private readonly IAuditLoggerInternal _helper;

    public ManualAuditLogger(IAuditLoggerInternal helper)
    {
        _helper = helper;
    }
}

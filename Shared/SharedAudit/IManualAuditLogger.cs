namespace SharedAudit;

public interface IManualAuditLogger
{
    void Log(
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
        IDictionary<string, string>? bodyAfter = null);
}

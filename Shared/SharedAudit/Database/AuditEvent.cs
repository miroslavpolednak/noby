namespace SharedAudit.Database;

internal sealed record AuditEvent(string EventID, string AuditEventTypeId, string Detail)
{
}

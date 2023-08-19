namespace CIS.Infrastructure.Audit.Database;

internal sealed record AuditEvent(string EventID, string AuditEventTypeId, string Detail)
{
}

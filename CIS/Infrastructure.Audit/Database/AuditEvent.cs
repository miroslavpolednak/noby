namespace CIS.Infrastructure.Audit.Database;

internal sealed record AuditEvent(Guid EventID, string AuditEventTypeId, string Detail)
{
}

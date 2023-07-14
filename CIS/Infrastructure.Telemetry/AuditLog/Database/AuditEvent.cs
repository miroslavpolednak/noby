namespace CIS.Infrastructure.Telemetry.AuditLog.Database;

internal sealed record AuditEvent(Guid EventID, string AuditEventTypeId, string Detail)
{
}

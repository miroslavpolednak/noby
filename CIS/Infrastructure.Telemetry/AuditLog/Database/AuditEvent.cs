namespace CIS.Infrastructure.Telemetry.AuditLog.Database;

internal record AuditEvent(int AuditEventTypeId, string Detail)
{
}

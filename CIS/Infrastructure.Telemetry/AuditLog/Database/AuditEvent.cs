namespace CIS.Infrastructure.Telemetry.AuditLog.Database;

internal record AuditEvent(Guid EventID, string AuditEventTypeId, string Detail)
{
}

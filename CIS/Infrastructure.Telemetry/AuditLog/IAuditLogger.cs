using CIS.Infrastructure.Telemetry.AuditLog;
using static CIS.Infrastructure.Telemetry.AuditLog.AuditLogger;

namespace CIS.Infrastructure.Telemetry;

public interface IAuditLogger
{
    void Log(
        AuditEventTypes eventType,
        ICollection<AuditLoggerHeaderItem>? identities = null,
        ICollection<AuditLoggerHeaderItem>? products = null,
        AuditLoggerHeaderItem? operation = null);
}
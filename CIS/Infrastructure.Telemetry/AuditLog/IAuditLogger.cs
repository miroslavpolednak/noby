using CIS.Infrastructure.Telemetry.AuditLog;

namespace CIS.Infrastructure.Telemetry;

public interface IAuditLogger
{
    void Log(AuditEventTypes eventType);
}

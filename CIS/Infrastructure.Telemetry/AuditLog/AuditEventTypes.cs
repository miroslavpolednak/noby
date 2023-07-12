using System.ComponentModel.DataAnnotations;

namespace CIS.Infrastructure.Telemetry.AuditLog;

public enum AuditEventTypes 
    : int
{
    [AuditEventTypeDescriptor("popis", "NOBY_001")]
    Noby001 = 1
}

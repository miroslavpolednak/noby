using CIS.Infrastructure.Telemetry.AuditLog.Attributes;
using System.ComponentModel.DataAnnotations;

namespace CIS.Infrastructure.Telemetry.AuditLog;

public enum AuditEventTypes 
    : int
{
    [AuditEventTypeDescriptor("LoginInitiated", "NOBY_001")]
    Noby001 = 1,

    [AuditEventTypeDescriptor("LoginSuccessful", "NOBY_002")]
    Noby002 = 2,

    [AuditEventTypeDescriptor("LogoutSuccessful", "NOBY_003")]
    Noby003 = 3,
}

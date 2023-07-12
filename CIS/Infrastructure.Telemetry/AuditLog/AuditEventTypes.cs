using System.ComponentModel.DataAnnotations;

namespace CIS.Infrastructure.Telemetry.AuditLog;

public enum AuditEventTypes 
    : int
{
    [Display(Name = "", ShortName = "")]
    Prvni = 1
}

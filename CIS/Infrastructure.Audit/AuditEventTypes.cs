using CIS.Infrastructure.Audit.Attributes;

namespace CIS.Infrastructure.Audit;

public enum AuditEventTypes 
    : int
{
    [AuditEventTypeDescriptor("LoginInitiated", "NOBY_001")]
    Noby001 = 1,

    [AuditEventTypeDescriptor("LoginSuccessful", "NOBY_002")]
    Noby002 = 2,

    [AuditEventTypeDescriptor("LogoutSuccessful", "NOBY_003")]
    Noby003 = 3,

    [AuditEventTypeDescriptor("CaseCancelled", "NOBY_004")]
    Noby004 = 4,

    [AuditEventTypeDescriptor("SalesArrangementSubmittedForProcessing", "NOBY_005")]
    Noby005 = 5,

    [AuditEventTypeDescriptor("IdentifiedClientAssignedToSalesArrangement", "NOBY_006")]
    Noby006 = 6,

    [AuditEventTypeDescriptor("AccessToUnownedCase", "NOBY_009")]
    Noby009 = 9,
}

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
    
    [AuditEventTypeDescriptor("DocumentMarkedAsSigned", "NOBY_007")]
    Noby007 = 7,
    
    [AuditEventTypeDescriptor("SignedDocumentCancelled", "NOBY_008")]
    Noby008 = 8,

    [AuditEventTypeDescriptor("AccessToUnownedCase", "NOBY_009")]
    Noby009 = 9,
    
    [AuditEventTypeDescriptor("DocumentPreviewBeforeSigning", "NOBY_010")]
    Noby010 = 10,
    
    [AuditEventTypeDescriptor("DocumentSendToClientBeforeSigning", "NOBY_011")]
    Noby011 = 11,
    
    [AuditEventTypeDescriptor("AuditableSMSNotificationRequestReceived", "NOBY_012")]
    Noby012 = 12,
    
    [AuditEventTypeDescriptor("SMSNotificationSentThroughMCS", "NOBY_013")]
    Noby013 = 13,
}

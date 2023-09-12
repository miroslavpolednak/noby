using CIS.Infrastructure.Audit.Attributes;

namespace CIS.Infrastructure.Audit;

public enum AuditEventTypes 
    : int
{
    [AuditEventTypeDescriptor("LoginInitiated", "AU_NOBY_001")]
    Noby001 = 1,

    [AuditEventTypeDescriptor("LoginSuccessful", "AU_NOBY_002")]
    Noby002 = 2,

    [AuditEventTypeDescriptor("LogoutSuccessful", "AU_NOBY_003")]
    Noby003 = 3,

    [AuditEventTypeDescriptor("CaseCancelled", "AU_NOBY_004")]
    Noby004 = 4,

    [AuditEventTypeDescriptor("SalesArrangementSubmittedForProcessing", "AU_NOBY_005")]
    Noby005 = 5,

    [AuditEventTypeDescriptor("IdentifiedClientAssignedToSalesArrangement", "AU_NOBY_006")]
    Noby006 = 6,
    
    [AuditEventTypeDescriptor("DocumentMarkedAsSigned", "AU_NOBY_007")]
    Noby007 = 7,
    
    [AuditEventTypeDescriptor("SignedDocumentCancelled", "AU_NOBY_008")]
    Noby008 = 8,

    [AuditEventTypeDescriptor("AccessToUnownedCase", "AU_NOBY_009")]
    Noby009 = 9,
    
    [AuditEventTypeDescriptor("DocumentPreviewBeforeSigning", "AU_NOBY_010")]
    Noby010 = 10,
    
    [AuditEventTypeDescriptor("DocumentSendToClientBeforeSigning", "AU_NOBY_011")]
    Noby011 = 11,
    
    [AuditEventTypeDescriptor("AuditableSMSNotificationRequestReceived", "AU_NOBY_012")]
    Noby012 = 12,
    
    [AuditEventTypeDescriptor("SMSNotificationSentThroughMCS", "AU_NOBY_013")]
    Noby013 = 13,
    
    [AuditEventTypeDescriptor("NotificationResultForAuditableSMSReceived", "AU_NOBY_013")]
    Noby014 = 14,
}

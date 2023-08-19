using cz.kb.osbs.mcs.notificationreport.eventapi.v3.report;
using DomainServices.CodebookService.Contracts.v1;
namespace CIS.InternalServices.NotificationService.Api.Services.AuditLog.Abstraction;

public interface ISmsAuditLogger
{
    Task LogHttpRequestResponse();

    void LogKafkaProduced(SmsNotificationTypesResponse.Types.SmsNotificationTypeItem smsType, Guid notificationId, string consumer);
    
    void LogKafkaProduceError(SmsNotificationTypesResponse.Types.SmsNotificationTypeItem smsType, string consumer);

    void LogKafkaResultReceived(SmsNotificationTypesResponse.Types.SmsNotificationTypeItem smsType, NotificationReport report);
}
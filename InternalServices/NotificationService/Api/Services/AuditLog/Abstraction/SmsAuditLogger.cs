using cz.kb.osbs.mcs.notificationreport.eventapi.v3.report;
using DomainServices.CodebookService.Contracts.v1;
using Microsoft.AspNetCore.Mvc;

namespace CIS.InternalServices.NotificationService.Api.Services.AuditLog.Abstraction;

public interface ISmsAuditLogger
{
    Task LogHttpRequest();

    Task LogHttpResponse(IActionResult? actionResult);

    void LogHttpException(Exception exception);

    void LogKafkaProducing(SmsNotificationTypesResponse.Types.SmsNotificationTypeItem smsType, string consumer);

    void LogKafkaProduced(SmsNotificationTypesResponse.Types.SmsNotificationTypeItem smsType, Guid notificationId, string consumer);
    
    void LogKafkaError(SmsNotificationTypesResponse.Types.SmsNotificationTypeItem smsType, string consumer);

    void LogKafkaResultReceived(NotificationReport report);
}
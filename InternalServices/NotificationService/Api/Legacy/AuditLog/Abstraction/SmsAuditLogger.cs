using cz.kb.osbs.mcs.notificationreport.eventapi.v3.report;
using DomainServices.CodebookService.Contracts.v1;
namespace CIS.InternalServices.NotificationService.Api.Legacy.AuditLog.Abstraction;

public interface ISmsAuditLogger
{
    Task LogHttpRequestResponse();

    void LogKafkaProduced(SmsNotificationTypesResponse.Types.SmsNotificationTypeItem smsType, Guid notificationId, string consumer, string? identity, string? identityScheme, long? caseId, string? customId, string? documentId, string? documentHash, string? hashAlgorithm);

    void LogKafkaProduceError(SmsNotificationTypesResponse.Types.SmsNotificationTypeItem smsType, string consumer, string? identity, string? identityScheme, long? caseId, string? customId, string? documentId, string? documentHash, string? hashAlgorithm);

    void LogKafkaResultReceived(SmsNotificationTypesResponse.Types.SmsNotificationTypeItem smsType, NotificationReport report, Guid notificationId);
}
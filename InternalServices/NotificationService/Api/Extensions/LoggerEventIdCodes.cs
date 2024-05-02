namespace CIS.InternalServices.NotificationService.Api;

internal sealed class LoggerEventIdCodes
{
    public const int NotificationSent = 300;
    public const int NotificationFailedToSend = 301;
    public const int SaveAttachmentFailed = 302;
    public const int NotificationRequestReceived = 303;
    public const int KafkaNotificationResultIdEmpty = 304;
    public const int KafkaNotificationResultNotificationNotFound = 305;
    public const int KafkaNotificationResultUnknownState = 306;
    public const int SendEmailsJobStart = 307;
    public const int SendEmailsJobPayloadFailed = 308;
    public const int SendEmailsJobValidationError = 309;
    public const int SendEmailsJobFailedToSend = 310;
    public const int SendEmailsJobEnd = 311;
    public const int SetExpiredEmailsJob = 312;
}
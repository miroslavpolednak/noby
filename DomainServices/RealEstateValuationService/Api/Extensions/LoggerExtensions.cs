namespace DomainServices.RealEstateValuationService.Api;

internal static class LoggerExtensions
{
    private static readonly Action<ILogger, long, int, Exception> _attachmentDeleteFailed;
    private static readonly Action<ILogger, string, Exception> _kafkaMessageCaseIdIncorrectFormat;
    private static readonly Action<ILogger, string, Exception> _kafkaMessageCurrentTaskIdIncorrectFormat;

    static LoggerExtensions()
    {
        _attachmentDeleteFailed = LoggerMessage.Define<long, int>(
            LogLevel.Warning,
            new EventId(LoggerEventIdCodes.AttachmentDeleteFailed, nameof(AttachmentDeleteFailed)),
            "Attachment {RealEstateValuationAttachmentId} with ExternalId {ExternalId} failed to delete in Preorder service");
        
        _kafkaMessageCaseIdIncorrectFormat = LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(LoggerEventIdCodes.KafkaMessageCaseIdIncorrectFormat, nameof(KafkaMessageCaseIdIncorrectFormat)),
            "Message CaseId {CaseId} is not in valid format");

        _kafkaMessageCurrentTaskIdIncorrectFormat = LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(LoggerEventIdCodes.KafkaMessageCurrentTaskIdIncorrectFormat, nameof(KafkaMessageCurrentTaskIdIncorrectFormat)),
            "Message CurrentTaskId {CurrentTaskId} is not in valid format");
    }

    public static void AttachmentDeleteFailed(this ILogger logger, long externalId, int realEstateValuationAttachmentId, Exception ex)
        => _attachmentDeleteFailed(logger, externalId, realEstateValuationAttachmentId, ex);
    
    public static void KafkaMessageCaseIdIncorrectFormat(this ILogger logger, string caseId)
        => _kafkaMessageCaseIdIncorrectFormat(logger, caseId, null!);

    public static void KafkaMessageCurrentTaskIdIncorrectFormat(this ILogger logger, string currentTaskId)
        => _kafkaMessageCurrentTaskIdIncorrectFormat(logger, currentTaskId, null!);
    
}

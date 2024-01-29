namespace DomainServices.RealEstateValuationService.Api;

internal static class LoggerExtensions
{
    private static readonly Action<ILogger, long, int, Exception> _attachmentDeleteFailed;
    private static readonly Action<ILogger, string, Exception> _kafkaMessageCaseIdIncorrectFormat;
    private static readonly Action<ILogger, string, Exception> _kafkaMessageCurrentTaskIdIncorrectFormat;
    private static readonly Action<ILogger, long?, Exception> _kafkaRealEstateValuationByOrderIdNotFound;
    private static readonly Action<ILogger, bool, Exception> _revaluationFinished;
    private static readonly Action<ILogger, bool, long, int, Exception> _createKbmodelFlat;

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
        
        _kafkaRealEstateValuationByOrderIdNotFound = LoggerMessage.Define<long?>(
            LogLevel.Warning,
            new EventId(LoggerEventIdCodes.RealEstateValuationNotFound, nameof(RealEstateValuationByOrderIdNotFound)),
            "RealEstateValuation OrderId {OrderId} not found");

        _revaluationFinished = LoggerMessage.Define<bool>(
            LogLevel.Information,
            new EventId(LoggerEventIdCodes.RevaluationFinished, nameof(RevaluationFinished)),
            "Revaluation finished with result {RevaluationRequired}");

        _createKbmodelFlat = LoggerMessage.Define<bool, long, int>(
            LogLevel.Information,
            new EventId(LoggerEventIdCodes.CreateKbmodelFlat, nameof(CreateKbmodelFlat)),
            "CreateKbmodelFlat: noPriceAvailable {NoPriceAvailable}; valuation {ValuationId} finished with result price {ResultPrice}");
    }

    public static void AttachmentDeleteFailed(this ILogger logger, in long externalId, in int realEstateValuationAttachmentId, Exception ex)
        => _attachmentDeleteFailed(logger, externalId, realEstateValuationAttachmentId, ex);
    
    public static void KafkaMessageCaseIdIncorrectFormat(this ILogger logger, in string caseId)
        => _kafkaMessageCaseIdIncorrectFormat(logger, caseId, null!);

    public static void KafkaMessageCurrentTaskIdIncorrectFormat(this ILogger logger, in string currentTaskId)
        => _kafkaMessageCurrentTaskIdIncorrectFormat(logger, currentTaskId, null!);

    public static void RealEstateValuationByOrderIdNotFound(this ILogger logger, in long? orderId)
        => _kafkaRealEstateValuationByOrderIdNotFound(logger, orderId, null!);

    public static void RevaluationFinished(this ILogger logger, in bool revaluationRequired)
        => _revaluationFinished(logger, revaluationRequired, null!);

    public static void CreateKbmodelFlat(this ILogger logger, in bool noPriceAvailable, in long valuationId, in int resultPrice)
        => _createKbmodelFlat(logger, noPriceAvailable, valuationId, resultPrice, null!);
}

using DomainServices.OfferService.Api.Extensions;

namespace DomainServices.OfferService.Api;

public static class LoggerExtensions
{
    private static readonly Action<ILogger, string, string, Exception> _kafkaMessageCaseIdIncorrectFormat
        = LoggerMessage.Define<string, string>(
            LogLevel.Error,
            new EventId(LoggerEventIdCodes.KafkaMessageCaseIdIncorrectFormat, nameof(KafkaMessageCaseIdIncorrectFormat)),
            "Kafka message processing in {Consumer}: CaseId {CaseId} is not in valid format");

    private static readonly Action<ILogger, string, string, Exception> _kafkaMessageCurrentTaskIdIncorrectFormat
        = LoggerMessage.Define<string, string>(
            LogLevel.Error,
            new EventId(LoggerEventIdCodes.KafkaMessageCurrentTaskIdIncorrectFormat, nameof(KafkaMessageCurrentTaskIdIncorrectFormat)),
            "Kafka message processing in {Consumer}: CurrentTaskId {CurrentTaskId} is not in valid format");

    private static readonly Action<ILogger, string, long, Exception> _kafkaCaseIdNotFound
        = LoggerMessage.Define<string, long>(
            LogLevel.Warning,
            new EventId(LoggerEventIdCodes.KafkaCaseIdNotFound, nameof(KafkaCaseIdNotFound)),
            "Kafka message processing in {Consumer}: Case {CaseId} not found");

    public static void KafkaMessageCaseIdIncorrectFormat(this ILogger logger, string consumerTypeName, string caseId)
        => _kafkaMessageCaseIdIncorrectFormat(logger, consumerTypeName, caseId, null!);

    public static void KafkaMessageCurrentTaskIdIncorrectFormat(this ILogger logger, string consumerTypeName, string currentTaskId)
        => _kafkaMessageCurrentTaskIdIncorrectFormat(logger, consumerTypeName, currentTaskId, null!);

    public static void KafkaCaseIdNotFound(this ILogger logger, string consumerTypeName, long caseId)
        => _kafkaCaseIdNotFound(logger, consumerTypeName, caseId, null!);
}
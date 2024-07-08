namespace DomainServices.DocumentOnSAService.Api;

public static class LoggerExtensions
{
    private static readonly Action<ILogger, string?, Exception> _kafkaSigningDocumentFailed;

    static LoggerExtensions()
    {
        _kafkaSigningDocumentFailed = LoggerMessage.Define<string?>(
            LogLevel.Debug,
            new EventId(0, nameof(KafkaSigningDocumentFailed)),
            "Signing failed for document '{DocumentExternalId}'");
    }

    public static void KafkaSigningDocumentFailed(this ILogger logger, string? documentExternalId, Exception exception)
        => _kafkaSigningDocumentFailed(logger, documentExternalId, exception);
}

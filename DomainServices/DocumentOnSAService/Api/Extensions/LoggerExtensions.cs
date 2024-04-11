namespace DomainServices.DocumentOnSAService.Api.Extensions;

public static class LoggerExtensions
{
    private static readonly Action<ILogger, int, Exception> _updateCustomerFailed;
    private static readonly Action<ILogger, long, Exception> _updateOfSbQueuesFailed;
    private static readonly Action<ILogger, Exception> _customExp;
    private static readonly Action<ILogger, int, Exception> _stopSigningError;

    static LoggerExtensions()
    {
        _updateCustomerFailed = LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(LoggerEventIdCodes.UpdateCustomerFailed, nameof(UpdateCustomerFailed)),
           "Update customerOnSa {CustomerOnSaId} failed");

        _updateOfSbQueuesFailed = LoggerMessage.Define<long>(
            LogLevel.Error,
            new EventId(LoggerEventIdCodes.UpdateOfSbQueuesFailed, nameof(UpdateOfSbQueuesFailed)),
           "Update of sb queues failed for documentId {DocumentId}");

        _customExp = LoggerMessage.Define(
            LogLevel.Error,
            new EventId(LoggerEventIdCodes.CustomExp, nameof(CustomExp)),
           "Exception was trow");

        _stopSigningError = LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(LoggerEventIdCodes.StopSigningError, nameof(UpdateCustomerFailed)),
           "For DocumentOnSaId:{DocumentOnSaId} error when call stop signing (Esignature)");
    }

    public static void CustomExp(this ILogger logger, Exception exception)
        => _customExp(logger, exception);

    public static void UpdateOfSbQueuesFailed(this ILogger logger, long documentId, Exception exception)
      => _updateOfSbQueuesFailed(logger, documentId, exception);

    public static void UpdateCustomerFailed(this ILogger logger, int customerOnSaId, Exception exception)
    => _updateCustomerFailed(logger, customerOnSaId, exception);

    public static void StopSigningError(this ILogger logger, int documentOnSaId, Exception exception)
    => _stopSigningError(logger, documentOnSaId, exception);
}

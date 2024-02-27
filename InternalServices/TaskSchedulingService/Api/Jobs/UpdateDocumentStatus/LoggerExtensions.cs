namespace CIS.InternalServices.TaskSchedulingService.Api.Jobs.UpdateDocumentStatus;

internal static class LoggerExtensions
{
    private static readonly Action<ILogger, int, Exception> _updateDocumentStatusFailed;

    static LoggerExtensions()
    {
        _updateDocumentStatusFailed = LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(606, nameof(UpdateDocumentStatusFailed)),
           "Update documentOnSa {DocumentId} failed");
    }

    public static void UpdateDocumentStatusFailed(this ILogger logger, in int documentId, Exception exception)
     => _updateDocumentStatusFailed(logger, documentId, exception);
}

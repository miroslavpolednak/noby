namespace DomainServices.DocumentOnSAService.Api.Extensions;

public static class LoggerExtensionsBG
{
    private static readonly Action<ILogger, string, int, Exception> _unarchivedDocumentsOnSa;
    private static readonly Action<ILogger, string, int, int, Exception> _alreadyArchived;
    private static readonly Action<ILogger, int, Exception> _updateDocumentStatusFailed;

    static LoggerExtensionsBG()
    {
        _unarchivedDocumentsOnSa = LoggerMessage.Define<string, int>(
          LogLevel.Information,
          new EventId(1, nameof(UnarchivedDocumentsOnSa)),
          "{ServiceName}: {Count} unarchived documentsOnSa");

        _alreadyArchived = LoggerMessage.Define<string, int, int>(
            LogLevel.Information,
            new EventId(2, nameof(AlreadyArchived)),
          "{ServiceName}:From {UnArchCount} unarchived documentsOnSa, {ArchCount} have been already archived");

        _updateDocumentStatusFailed = LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(3, nameof(UpdateDocumentStatusFailed)),
           "Update documentOnSa {DocumentId} failed");
    }

    public static void UnarchivedDocumentsOnSa(this ILogger logger, string serviceName, int count)
     => _unarchivedDocumentsOnSa(logger, serviceName, count, default!);

    public static void AlreadyArchived(this ILogger logger, string serviceName, int unArchivedCount, int archivedCount)
     => _alreadyArchived(logger, serviceName, unArchivedCount, archivedCount, default!);

    public static void UpdateDocumentStatusFailed(this ILogger logger, int documentId, Exception exception)
     => _updateDocumentStatusFailed(logger, documentId, exception);
}

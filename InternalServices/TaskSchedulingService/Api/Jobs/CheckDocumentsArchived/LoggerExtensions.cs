namespace CIS.InternalServices.TaskSchedulingService.Api.Jobs.CheckDocumentsArchived;

internal static class LoggerExtensions
{
    private static readonly Action<ILogger, int, Exception> _unarchivedDocumentsOnSa;
    private static readonly Action<ILogger, int, int, Exception> _alreadyArchived;

    static LoggerExtensions()
    {
        _unarchivedDocumentsOnSa = LoggerMessage.Define<int>(
          LogLevel.Information,
          new EventId(608, nameof(UnarchivedDocumentsOnSa)),
          "CheckDocumentsArchivedHandler: {Count} unarchived documentsOnSa");

        _alreadyArchived = LoggerMessage.Define<int, int>(
            LogLevel.Information,
            new EventId(609, nameof(AlreadyArchived)),
          "CheckDocumentsArchivedHandler:From {UnArchCount} unarchived documentsOnSa, {ArchCount} have been already archived");
    }

    public static void UnarchivedDocumentsOnSa(this ILogger logger, in int count)
        => _unarchivedDocumentsOnSa(logger, count, default!);

    public static void AlreadyArchived(this ILogger logger, in int unArchivedCount, in int archivedCount)
        => _alreadyArchived(logger, unArchivedCount, archivedCount, default!);

}

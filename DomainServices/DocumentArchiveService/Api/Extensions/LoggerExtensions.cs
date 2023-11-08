namespace DomainServices.DocumentArchiveService.Api.Extensions;

internal static class LoggerExtensions
{
    private static readonly Action<ILogger, Exception> _localDocCopyIfNotExistInEArchive;


    static LoggerExtensions()
    {
        _localDocCopyIfNotExistInEArchive = LoggerMessage.Define(
            LogLevel.Error,
            new EventId(LoggerEventIdCodes.LocalDocCopyIfNotExistInEArchive, nameof(LocalDocCopyIfNotExistInEArchive)),
            "Exception when get data from EArchive, local data were returned");
    }


    public static void LocalDocCopyIfNotExistInEArchive(this ILogger logger, Exception exception)
     => _localDocCopyIfNotExistInEArchive(logger, exception);

}

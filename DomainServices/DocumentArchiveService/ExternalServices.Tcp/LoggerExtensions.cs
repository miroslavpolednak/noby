using Microsoft.Extensions.Logging;

namespace DomainServices.DocumentArchiveService.ExternalServices.Tcp;
internal static class LoggerExtensions
{
    private static readonly Action<ILogger, string, Exception> _errorWhenDownloadingFile;

    static LoggerExtensions()
    {
        _errorWhenDownloadingFile = LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(1001, nameof(ErrorWhenDownloadingFile)),
            "Error when downloading file, {Result}");
    }

    public static void ErrorWhenDownloadingFile(this ILogger logger, string result)
     => _errorWhenDownloadingFile(logger, result, null!);
}

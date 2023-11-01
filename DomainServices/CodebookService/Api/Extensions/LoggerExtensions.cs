namespace DomainServices.CodebookService.Api;

internal static class LoggerExtensions
{
    private static readonly Action<ILogger, string, Exception> _rdmCodebookLoading;
    private static readonly Action<ILogger, int, string, Exception> _rdmCodebookLoaded;
    private static readonly Action<ILogger, string, Exception> _rdmCodebookLoadingException;

    static LoggerExtensions()
    {
        _rdmCodebookLoading = LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(9001, nameof(RdmCodebookLoading)),
            "RDM Codebook {CodebookName} being fetched");

        _rdmCodebookLoaded = LoggerMessage.Define<int, string>(
            LogLevel.Information,
            new EventId(9002, nameof(RdmCodebookLoaded)),
            "{Rows} rows of RDM Codebook {CodebookName} saved");

        _rdmCodebookLoadingException = LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(9003, nameof(RdmCodebookLoadingException)),
            "RDM Codebook {CodebookName} load failed");
    }

    public static void RdmCodebookLoading(this ILogger logger, string codebookName)
        => _rdmCodebookLoading(logger, codebookName, null!);

    public static void RdmCodebookLoaded(this ILogger logger, string codebookName, int rows)
        => _rdmCodebookLoaded(logger, rows, codebookName, null!);

    public static void RdmCodebookLoadingException(this ILogger logger, string codebookName, Exception ex)
        => _rdmCodebookLoadingException(logger, codebookName, ex);
}

namespace CIS.Infrastructure.ExternalServicesHelpers;

internal static class LoggerExtensions
{
    private static readonly Action<ILogger, string, int, Exception> _httpRequestRetry;

    static LoggerExtensions()
    {
        _httpRequestRetry = LoggerMessage.Define<string, int>(
            LogLevel.Information,
            new EventId(740, nameof(HttpRequestRetry)),
            "{Client}: #{Count} Retry HttpRequest");
    }

    public static void HttpRequestRetry(this ILogger logger, string client, int count)
        => _httpRequestRetry(logger, client, count, null!);
}
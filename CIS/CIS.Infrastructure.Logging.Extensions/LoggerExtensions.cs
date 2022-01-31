using Microsoft.Extensions.Logging;

namespace CIS.Infrastructure.Logging;

public static class LoggerExtensions
{
    private static readonly Action<ILogger, int, Exception> _foundItems;
    private static readonly Action<ILogger, string, string, Exception> _generalException;
    private static readonly Action<ILogger, string, Exception> _generalException2;
    private static readonly Action<ILogger, int, string, string, Exception> _invalidArgument;

    static LoggerExtensions()
    {
        _foundItems = LoggerMessage.Define<int>(
            LogLevel.Debug,
            new EventId(506, nameof(FoundItems)),
            "Found {Count} items");

        _generalException = LoggerMessage.Define<string, string>(
            LogLevel.Debug,
            new EventId(510, nameof(GeneralException)),
            "Uncought Exception in {MethodName}: {Message}");

        _generalException2 = LoggerMessage.Define<string>(
            LogLevel.Debug,
            new EventId(511, nameof(GeneralException)),
            "Uncought Exception: {Message}");

        _invalidArgument = LoggerMessage.Define<int, string, string>(
            LogLevel.Debug,
            new EventId(512, nameof(GeneralException)),
            "{Code} in {ArgumentName}: {Message}");
    }

    public static void FoundItems(this ILogger logger, int count)
        => _foundItems(logger, count, null!);

    public static void GeneralException(this ILogger logger, string methodName, string message, Exception ex)
        => _generalException(logger, methodName, message, ex);

    public static void GeneralException(this ILogger logger, Exception ex)
        => _generalException2(logger, ex.Message, ex);

    public static void InvalidArgument(this ILogger logger, int code, string argumentName, string message, Exception ex)
        => _invalidArgument(logger, code, argumentName, message, ex);
}

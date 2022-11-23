using Microsoft.Extensions.Logging;
using System.Collections.Immutable;

namespace CIS.Infrastructure.Logging;

public static class ExceptionLoggerExtensions
{
    private static readonly Action<ILogger, string, string, Exception> _generalException;
    private static readonly Action<ILogger, string, Exception> _generalException2;
    private static readonly Action<ILogger, int, string, string, Exception> _invalidArgument;
    private static readonly Action<ILogger, string, Exception> _validationException;

    static ExceptionLoggerExtensions()
    {
        _generalException = LoggerMessage.Define<string, string>(
            LogLevel.Debug,
            new EventId(EventIdCodes.GeneralException, nameof(GeneralException)),
            "Uncought Exception in {MethodName}: {Message}");

        _generalException2 = LoggerMessage.Define<string>(
            LogLevel.Debug,
            new EventId(EventIdCodes.GeneralException2, nameof(GeneralException)),
            "Uncought Exception: {Message}");

        _invalidArgument = LoggerMessage.Define<int, string, string>(
            LogLevel.Debug,
            new EventId(EventIdCodes.InvalidArgument, nameof(InvalidArgument)),
            "{Code} in {ArgumentName}: {Message}");
        
        _validationException = LoggerMessage.Define<string>(
            LogLevel.Debug,
            new EventId(EventIdCodes.ValidationException, nameof(ValidationException)),
            "Validation exceptions: {Messages}");
    }

    public static void GeneralException(this ILogger logger, string methodName, string message, Exception ex)
        => _generalException(logger, methodName, message, ex);

    public static void GeneralException(this ILogger logger, Exception ex)
        => _generalException2(logger, ex.Message, ex);

    public static void InvalidArgument(this ILogger logger, int code, string argumentName, string message, Exception ex)
        => _invalidArgument(logger, code, argumentName, message, ex);
    
    public static void ValidationException(this ILogger logger, IEnumerable<(string Key, string Message)> messages)
        => _validationException(logger, string.Join(";", messages.Select(t => $"{t.Key}: {t.Message}")), null!);

    public static void ValidationException(this ILogger logger, IEnumerable<(int Key, string Message)> messages)
        => _validationException(logger, string.Join(";", messages.Select(t => $"{t.Key}: {t.Message}")), null!);

    public static void ValidationException(this ILogger logger, Core.Exceptions.CisValidationException exception)
        => _validationException(logger, string.Join(";", exception.Errors!.Select(t => $"{t.Key}: {t.Message}")), null!);
}
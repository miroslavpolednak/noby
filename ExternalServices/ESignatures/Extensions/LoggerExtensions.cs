using Microsoft.Extensions.Logging;

namespace ExternalServices.ESignatures.Extensions;
public static class LoggerExtensions
{
    private static readonly Action<ILogger, int, Exception> _submitDispatchFormIgnoreError;
    
    static LoggerExtensions()
    {
        _submitDispatchFormIgnoreError = LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(LoggerEventIdCodes.SubmitDispatchFormIgnoreError, nameof(SubmitDispatchFormIgnoreError)),
            "SubmitDispatchForm return error code {ErrorCode}");
    }

    public static void SubmitDispatchFormIgnoreError(this ILogger logger, int errorCode)
          => _submitDispatchFormIgnoreError(logger, errorCode, null!);
}

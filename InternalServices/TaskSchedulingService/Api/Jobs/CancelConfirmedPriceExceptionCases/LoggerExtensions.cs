namespace CIS.InternalServices.TaskSchedulingService.Api.Jobs.CancelConfirmedPriceExceptionCases;

internal static class LoggerExtensions
{
    private static readonly Action<ILogger, int, DateTime, Exception> _casesToCancel;
    private static readonly Action<ILogger, long, Exception> _tryToCancelCase;
    private static readonly Action<ILogger, long, Exception> _failedToCancelCase;

    static LoggerExtensions()
    {
        _casesToCancel = LoggerMessage.Define<int, DateTime>(
            LogLevel.Information,
            new EventId(640, nameof(CasesToCancel)),
            "Found {Count} Cases older than {OlderThan}");

        _tryToCancelCase = LoggerMessage.Define<long>(
            LogLevel.Information,
            new EventId(641, nameof(TryToCancelCase)),
            "Calling CancelCase for Case {CaseId}");

        _failedToCancelCase = LoggerMessage.Define<long>(
            LogLevel.Error,
            new EventId(642, nameof(FailedToCancelCase)),
            "Unable to cancel Case {CaseId}");
    }

    public static void CasesToCancel(this ILogger logger, int count, DateTime olderThan)
        => _casesToCancel(logger, count, olderThan, null!);

    public static void TryToCancelCase(this ILogger logger, long caseId)
        => _tryToCancelCase(logger, caseId, null!);

    public static void FailedToCancelCase(this ILogger logger, long caseId, Exception ex)
        => _failedToCancelCase(logger, caseId, ex);
}

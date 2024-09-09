namespace CIS.InternalServices.TaskSchedulingService.Api.Jobs.CancelSelectedPriceExceptionCases;

internal static class LoggerExtensions
{
    private static readonly Action<ILogger, int, DateTime, Exception> _casesToCancel = LoggerMessage.Define<int, DateTime>(
        LogLevel.Information,
        new EventId(640, nameof(CasesToCancel)),
        "Found {Count} Cases older than {OlderThan}");

    private static readonly Action<ILogger, long, Exception> _tryToCancelCase = LoggerMessage.Define<long>(
        LogLevel.Information,
        new EventId(641, nameof(TryToCancelCase)),
        "Calling CancelCase for Case {CaseId}");

    private static readonly Action<ILogger, long, Exception> _failedToCancelCase = LoggerMessage.Define<long>(
        LogLevel.Error,
        new EventId(642, nameof(FailedToCancelCase)),
        "Unable to cancel Case {CaseId}");

    private static readonly Action<ILogger, long, Exception> _failedToCancelCaseDeleted = LoggerMessage.Define<long>(
        LogLevel.Warning,
        new EventId(643, nameof(FailedToCancelCaseDeleted)),
        "Unable to cancel Case {CaseId}, but price exeptions were deleted from database");

    public static void CasesToCancel(this ILogger logger, int count, DateTime olderThan)
        => _casesToCancel(logger, count, olderThan, null!);

    public static void TryToCancelCase(this ILogger logger, long caseId)
        => _tryToCancelCase(logger, caseId, null!);

    public static void FailedToCancelCase(this ILogger logger, long caseId, Exception ex)
        => _failedToCancelCase(logger, caseId, ex);

    public static void FailedToCancelCaseDeleted(this ILogger logger, long caseId, Exception ex)
        => _failedToCancelCaseDeleted(logger, caseId, ex);
}

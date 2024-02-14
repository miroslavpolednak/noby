namespace CIS.InternalServices.TaskSchedulingService.Api.Jobs.CancelCaseBySalesArrangementConditions;

internal static class LoggerExtensions
{
    private static readonly Action<ILogger, long, string, Exception> _cancelCaseJobFailed;
    private static readonly Action<ILogger, long, Exception> _cancelCaseJobFinished;
    private static readonly Action<ILogger, long, string, Exception> _cancelCaseJobSkipped;

    static LoggerExtensions()
    {
        _cancelCaseJobFailed = LoggerMessage.Define<long, string>(
            LogLevel.Warning,
            new EventId(630, nameof(CancelCaseJobFailed)),
            "CancelCase job failed for CaseId '{CaseId}': {Message}");

        _cancelCaseJobFinished = LoggerMessage.Define<long>(
            LogLevel.Information,
            new EventId(631, nameof(CancelCaseJobFinished)),
            "CancelCase job finished for CaseId '{CaseId}'");

        _cancelCaseJobSkipped = LoggerMessage.Define<long, string>(
            LogLevel.Information,
            new EventId(632, nameof(CancelCaseJobSkipped)),
            "CancelCase job skipped for CaseId '{CaseId}' due to {Reason}");
    }

    public static void CancelCaseJobFailed(this ILogger logger, long caseId, string message, Exception ex)
        => _cancelCaseJobFailed(logger, caseId, message, ex);

    public static void CancelCaseJobFinished(this ILogger logger, long caseId)
        => _cancelCaseJobFinished(logger, caseId, null!);

    public static void CancelCaseJobSkipped(this ILogger logger, long caseId, string reason)
        => _cancelCaseJobSkipped(logger, caseId, reason, null!);
}

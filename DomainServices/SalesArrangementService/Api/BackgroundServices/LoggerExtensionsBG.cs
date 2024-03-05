namespace DomainServices.SalesArrangementService.Api;

internal static class LoggerExtensionsBG
{
    private static readonly Action<ILogger, int, Exception> _deleteServiceSalesArrangements;
    private static readonly Action<ILogger, long, string, Exception> _cancelCaseJobFailed;
    private static readonly Action<ILogger, long, Exception> _cancelCaseJobFinished;
    private static readonly Action<ILogger, long, string, Exception> _cancelCaseJobSkipped;
    private static readonly Action<ILogger, long, int, Exception> _offerGuaranteeDateToCheckJobCancelTask;
    private static readonly Action<ILogger, int, Exception> _offerGuaranteeDateToCheckJobFinished;

    static LoggerExtensionsBG()
    {
        _deleteServiceSalesArrangements = LoggerMessage.Define<int>(
            LogLevel.Information,
            new EventId(5, nameof(DeleteServiceSalesArrangement)),
            "{SaForDeleteCount} SalesArrangements gonna be deleted");

        _cancelCaseJobFailed = LoggerMessage.Define<long, string>(
            LogLevel.Warning,
            new EventId(4, nameof(CancelCaseJobFailed)),
            "CancelCase job failed for CaseId '{CaseId}': {Message}");

        _cancelCaseJobFinished = LoggerMessage.Define<long>(
            LogLevel.Information,
            new EventId(3, nameof(CancelCaseJobFinished)),
            "CancelCase job finished for CaseId '{CaseId}'");

        _cancelCaseJobSkipped = LoggerMessage.Define<long, string>(
            LogLevel.Information,
            new EventId(2, nameof(CancelCaseJobSkipped)),
            "CancelCase job skipped for CaseId '{CaseId}' due to {Reason}");

        _offerGuaranteeDateToCheckJobCancelTask = LoggerMessage.Define<long, int>(
            LogLevel.Information,
            new EventId(2, nameof(OfferGuaranteeDateToCheckJobCancelTask)),
            "OfferGuaranteeDateToCheckJob is cancelled task for case {CaseId} with SB ID {TaskSbId}");

        _offerGuaranteeDateToCheckJobFinished = LoggerMessage.Define<int>(
            LogLevel.Information,
            new EventId(1, nameof(OfferGuaranteeDateToCheckJobFinished)),
            "OfferGuaranteeDateToCheckJob set switch 1 to false on SalesArrangement {SalesArrangementId}");
    }

    public static void DeleteServiceSalesArrangement(this ILogger logger, int SaForDeleteCount)
        => _deleteServiceSalesArrangements(logger, SaForDeleteCount, null!);

    public static void CancelCaseJobFailed(this ILogger logger, long caseId, string message, Exception ex)
        => _cancelCaseJobFailed(logger, caseId, message, ex);

    public static void CancelCaseJobFinished(this ILogger logger, long caseId)
        => _cancelCaseJobFinished(logger, caseId, null!);

    public static void CancelCaseJobSkipped(this ILogger logger, long caseId, string reason)
        => _cancelCaseJobSkipped(logger, caseId, reason, null!);

    public static void OfferGuaranteeDateToCheckJobCancelTask(this ILogger logger, long caseId, int taskSbId)
        => _offerGuaranteeDateToCheckJobCancelTask(logger, caseId, taskSbId, null!);

    public static void OfferGuaranteeDateToCheckJobFinished(this ILogger logger, int salesArrangementId)
        => _offerGuaranteeDateToCheckJobFinished(logger, salesArrangementId, null!);
}

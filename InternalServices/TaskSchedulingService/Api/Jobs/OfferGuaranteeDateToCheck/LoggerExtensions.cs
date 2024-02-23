namespace CIS.InternalServices.TaskSchedulingService.Api.Jobs.OfferGuaranteeDateToCheck;

internal static class LoggerExtensions
{
    private static readonly Action<ILogger, long, int, Exception> _offerGuaranteeDateToCheckJobCancelTask;
    private static readonly Action<ILogger, int, Exception> _offerGuaranteeDateToCheckJobFinished;

    static LoggerExtensions()
    {
        _offerGuaranteeDateToCheckJobCancelTask = LoggerMessage.Define<long, int>(
            LogLevel.Information,
            new EventId(605, nameof(OfferGuaranteeDateToCheckJobCancelTask)),
            "OfferGuaranteeDateToCheckJob is cancelled task for case {CaseId} with SB ID {TaskSbId}");

        _offerGuaranteeDateToCheckJobFinished = LoggerMessage.Define<int>(
            LogLevel.Information,
            new EventId(606, nameof(OfferGuaranteeDateToCheckJobFinished)),
            "OfferGuaranteeDateToCheckJob set switch 1 to false on SalesArrangement {SalesArrangementId}");
    }

    public static void OfferGuaranteeDateToCheckJobCancelTask(this ILogger logger, in long caseId, in int taskSbId)
        => _offerGuaranteeDateToCheckJobCancelTask(logger, caseId, taskSbId, null!);

    public static void OfferGuaranteeDateToCheckJobFinished(this ILogger logger, in int salesArrangementId)
        => _offerGuaranteeDateToCheckJobFinished(logger, salesArrangementId, null!);
}

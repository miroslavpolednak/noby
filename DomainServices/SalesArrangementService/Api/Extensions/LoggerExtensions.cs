namespace DomainServices.SalesArrangementService.Api;

internal static class LoggerExtensions
{
    private static readonly Action<ILogger, int, long, int?, Exception> _createSalesArrangementStarted;
    private static readonly Action<ILogger, int, int, Exception> _linkToModelationStarted;
    private static readonly Action<ILogger, int, int, Exception> _updateStateStarted;
    private static readonly Action<ILogger, int, Exception> _deleteServiceSalesArrangements;
    private static readonly Action<ILogger, long, string, Exception> _cancelCaseJobFailed;
    private static readonly Action<ILogger, long, Exception> _cancelCaseJobFinished;
    private static readonly Action<ILogger, long, Exception> _cancelCaseJobSkipped;
    private static readonly Action<ILogger, long, int, Exception> _offerGuaranteeDateToCheckJobCancelTask;
    private static readonly Action<ILogger, int, Exception> _offerGuaranteeDateToCheckJobFinished;

    static LoggerExtensions()
    {
        _createSalesArrangementStarted = LoggerMessage.Define<int, long, int?>(
            LogLevel.Debug,
            new EventId(LoggerEventIdCodes.CreateSalesArrangementStarted, nameof(CreateSalesArrangementStarted)),
            "Create SA {SalesArrangementTypeId} for #{CaseId}/#{OfferId}");

        _linkToModelationStarted = LoggerMessage.Define<int, int>(
            LogLevel.Debug,
            new EventId(LoggerEventIdCodes.LinkToModelationStarted, nameof(LinkToModelationStarted)),
            "Link Offer {OfferId} to {SalesArrangementId}");

        _updateStateStarted = LoggerMessage.Define<int, int>(
            LogLevel.Debug,
            new EventId(LoggerEventIdCodes.UpdateStateStarted, nameof(UpdateStateStarted)),
            "Update SA #{SalesArrangementId} State to {State}");

        _deleteServiceSalesArrangements = LoggerMessage.Define<int>(
            LogLevel.Information,
            new EventId(LoggerEventIdCodes.DeleteServiceSalesArrangement, nameof(DeleteServiceSalesArrangement)),
            "{SaForDeleteCount} SalesArrangements gonna be deleted");

        _cancelCaseJobFailed = LoggerMessage.Define<long, string>(
            LogLevel.Warning,
            new EventId(LoggerEventIdCodes.CancelCaseJobFailed, nameof(CancelCaseJobFailed)),
            "CancelCase job failed for CaseId '{CaseId}': {Message}");

        _cancelCaseJobFinished = LoggerMessage.Define<long>(
            LogLevel.Information,
            new EventId(LoggerEventIdCodes.CancelCaseJobFinished, nameof(CancelCaseJobFinished)),
            "CancelCase job finished for CaseId '{CaseId}'");

        _cancelCaseJobSkipped = LoggerMessage.Define<long>(
            LogLevel.Information,
            new EventId(LoggerEventIdCodes.CancelCaseJobSkipped, nameof(CancelCaseJobSkipped)),
            "CancelCase job skipped due to CaseState for CaseId '{CaseId}'");

        _offerGuaranteeDateToCheckJobCancelTask = LoggerMessage.Define<long, int>(
            LogLevel.Information,
            new EventId(LoggerEventIdCodes.OfferGuaranteeDateToCheckJobCancelTask, nameof(OfferGuaranteeDateToCheckJobCancelTask)),
            "OfferGuaranteeDateToCheckJob is cancelled task for case {CaseId} with SB ID {TaskSbId}");

        _offerGuaranteeDateToCheckJobFinished = LoggerMessage.Define<int>(
            LogLevel.Information,
            new EventId(LoggerEventIdCodes.OfferGuaranteeDateToCheckJobCancelTask, nameof(OfferGuaranteeDateToCheckJobFinished)),
            "OfferGuaranteeDateToCheckJob set switch 1 to false on SalesArrangement {SalesArrangementId}");
    }

    public static void CreateSalesArrangementStarted(this ILogger logger, int salesArrangementTypeId, long caseId, int? offerId)
        => _createSalesArrangementStarted(logger, salesArrangementTypeId, caseId, offerId, null!);

    public static void LinkToModelationStarted(this ILogger logger, int offerId, int salesArrangementId)
        => _linkToModelationStarted(logger, offerId, salesArrangementId, null!);

    public static void UpdateStateStarted(this ILogger logger, int salesArrangementId, int state)
        => _updateStateStarted(logger, salesArrangementId, state, null!);

    public static void DeleteServiceSalesArrangement(this ILogger logger, int SaForDeleteCount)
        => _deleteServiceSalesArrangements(logger, SaForDeleteCount, null!);

    public static void CancelCaseJobFailed(this ILogger logger, long caseId, string message, Exception ex)
        => _cancelCaseJobFailed(logger, caseId, message, ex);

    public static void CancelCaseJobFinished(this ILogger logger, long caseId)
        => _cancelCaseJobFinished(logger, caseId, null!);

    public static void CancelCaseJobSkipped(this ILogger logger, long caseId)
        => _cancelCaseJobSkipped(logger, caseId, null!);

    public static void OfferGuaranteeDateToCheckJobCancelTask(this ILogger logger, long caseId, int taskSbId)
        => _offerGuaranteeDateToCheckJobCancelTask(logger, caseId, taskSbId, null!);

    public static void OfferGuaranteeDateToCheckJobFinished(this ILogger logger, int salesArrangementId)
        => _offerGuaranteeDateToCheckJobFinished(logger, salesArrangementId, null!);
}

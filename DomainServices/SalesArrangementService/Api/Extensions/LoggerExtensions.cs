namespace DomainServices.SalesArrangementService.Api;

internal static class LoggerExtensions
{
    private static readonly Action<ILogger, int, long, int?, Exception> _createSalesArrangementStarted
        = LoggerMessage.Define<int, long, int?>(
            LogLevel.Debug,
            new EventId(LoggerEventIdCodes.CreateSalesArrangementStarted, nameof(CreateSalesArrangementStarted)),
            "Create SA {SalesArrangementTypeId} for #{CaseId}/#{OfferId}");

    private static readonly Action<ILogger, int, int, Exception> _linkToModelationStarted
        = LoggerMessage.Define<int, int>(
            LogLevel.Debug,
            new EventId(LoggerEventIdCodes.LinkToModelationStarted, nameof(LinkToModelationStarted)),
            "Link Offer {OfferId} to {SalesArrangementId}");

    private static readonly Action<ILogger, int, int, Exception> _updateStateStarted
        = LoggerMessage.Define<int, int>(
            LogLevel.Debug,
            new EventId(LoggerEventIdCodes.UpdateStateStarted, nameof(UpdateStateStarted)),
            "Update SA #{SalesArrangementId} State to {State}");

    private static readonly Action<ILogger, long, string, Exception> _cancelNotFinishedExtraPaymentsFailed
        = LoggerMessage.Define<long, string>(
            LogLevel.Warning,
            new EventId(LoggerEventIdCodes.CancelNotFinishedExtraPaymentsFailed, nameof(CancelNotFinishedExtraPaymentsFailed)),
            "CancelNotFinishedExtraPayments exception for Case #{CaseId}: {Message}");

    public static void CreateSalesArrangementStarted(this ILogger logger, int salesArrangementTypeId, long caseId, int? offerId)
        => _createSalesArrangementStarted(logger, salesArrangementTypeId, caseId, offerId, null!);

    public static void LinkToModelationStarted(this ILogger logger, int offerId, int salesArrangementId)
        => _linkToModelationStarted(logger, offerId, salesArrangementId, null!);

    public static void UpdateStateStarted(this ILogger logger, int salesArrangementId, int state)
        => _updateStateStarted(logger, salesArrangementId, state, null!);

    public static void CancelNotFinishedExtraPaymentsFailed(this ILogger logger, long caseId, string message, Exception ex)
        => _cancelNotFinishedExtraPaymentsFailed(logger, caseId, message, ex);
}

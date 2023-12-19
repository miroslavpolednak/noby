namespace DomainServices.SalesArrangementService.Api;

internal static class LoggerExtensions
{
    private static readonly Action<ILogger, int, long, int?, Exception> _createSalesArrangementStarted;
    private static readonly Action<ILogger, int, int, Exception> _linkToModelationStarted;
    private static readonly Action<ILogger, int, int, Exception> _updateStateStarted;
    private static readonly Action<ILogger, int, Exception> _deleteServiceSalesArrangements;

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
    }

    public static void CreateSalesArrangementStarted(this ILogger logger, int salesArrangementTypeId, long caseId, int? offerId)
        => _createSalesArrangementStarted(logger, salesArrangementTypeId, caseId, offerId, null!);

    public static void LinkToModelationStarted(this ILogger logger, int offerId, int salesArrangementId)
        => _linkToModelationStarted(logger, offerId, salesArrangementId, null!);

    public static void UpdateStateStarted(this ILogger logger, int salesArrangementId, int state)
        => _updateStateStarted(logger, salesArrangementId, state, null!);

    public static void DeleteServiceSalesArrangement(this ILogger logger, int SaForDeleteCount)
        => _deleteServiceSalesArrangements(logger, SaForDeleteCount, null!);
}

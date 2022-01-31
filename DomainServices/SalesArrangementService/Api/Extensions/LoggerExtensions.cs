namespace DomainServices.SalesArrangementService.Api;

internal static class LoggerExtensions
{
    private static readonly Action<ILogger, int, long, int?, Exception> _createSalesArrangementStarted;
    private static readonly Action<ILogger, int, int, Exception> _linkToModelationStarted;
    private static readonly Action<ILogger, int, int, Exception> _updateStateStarted;

    static LoggerExtensions()
    {
        _createSalesArrangementStarted = LoggerMessage.Define<int, long, int?>(
            LogLevel.Debug,
            new EventId(16501, nameof(CreateSalesArrangementStarted)),
            "Create SA {SalesArrangementTypeId} for #{CaseId}/#{OfferInstanceId}");

        _linkToModelationStarted = LoggerMessage.Define<int, int>(
            LogLevel.Debug,
            new EventId(16502, nameof(LinkToModelationStarted)),
            "Link OfferInstance {OfferInstanceId} to {SalesArrangementId}");

        _updateStateStarted = LoggerMessage.Define<int, int>(
            LogLevel.Debug,
            new EventId(16502, nameof(UpdateStateStarted)),
            "Update SA #{SalesArrangementId} State to {State}");
    }

    public static void CreateSalesArrangementStarted(this ILogger logger, int salesArrangementTypeId, long caseId, int? offerInstanceId)
        => _createSalesArrangementStarted(logger, salesArrangementTypeId, caseId, offerInstanceId, null!);

    public static void LinkToModelationStarted(this ILogger logger, int offerInstanceId, int salesArrangementId)
        => _linkToModelationStarted(logger, offerInstanceId, salesArrangementId, null!);

    public static void UpdateStateStarted(this ILogger logger, int salesArrangementId, int state)
        => _updateStateStarted(logger, salesArrangementId, state, null!);
}

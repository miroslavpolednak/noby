namespace DomainServices.SalesArrangementService.Api;

internal static class LoggerExtensions
{
    private static readonly Action<ILogger, int, long, int?, Exception> _createSalesArrangementStarted;

    static LoggerExtensions()
    {
        _createSalesArrangementStarted = LoggerMessage.Define<int, long, int?>(
            LogLevel.Debug,
            new EventId(16501, nameof(CreateSalesArrangementStarted)),
            "Create SA {SalesArrangementTypeId} for #{CaseId}/#{OfferInstanceId}");
    }

    public static void CreateSalesArrangementStarted(this ILogger logger, int salesArrangementTypeId, long caseId, int? offerInstanceId)
        => _createSalesArrangementStarted(logger, salesArrangementTypeId, caseId, offerInstanceId, null!);

}

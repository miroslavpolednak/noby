namespace FOMS.Api.Endpoints.Offer;

internal static class OfferApiModuleLoggerExtensions
{
    private static readonly Action<ILogger, Dto.SimulateMortgageRequest, Exception> _simulateMortgageStarted;

    static OfferApiModuleLoggerExtensions()
    {
        _simulateMortgageStarted = LoggerMessage.Define<Dto.SimulateMortgageRequest>(
            LogLevel.Debug,
            new EventId(90101, nameof(SimulateMortgageStarted)),
            "Case {CaseId} created");
    }

    public static void SimulateMortgageStarted(this ILogger logger, Dto.SimulateMortgageRequest request)
        => _simulateMortgageStarted(logger, request, null!);
}

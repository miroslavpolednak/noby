namespace FOMS.Api.Endpoints.Offer;

internal static class OfferApiModuleLoggerExtensions
{
    private static readonly Action<ILogger, SimulateMortgage.SimulateMortgageRequest, Exception> _simulateMortgageStarted;
    private static readonly Action<ILogger, SimulateMortgage.SimulateMortgageResponse, Exception> _simulateMortgageResult;

    static OfferApiModuleLoggerExtensions()
    {
        _simulateMortgageStarted = LoggerMessage.Define<SimulateMortgage.SimulateMortgageRequest>(
            LogLevel.Debug,
            new EventId(LoggerEventIdCodes.OfferSimulateMortgageStarted, nameof(OfferSimulateMortgageStarted)),
            "Case {CaseId} created");
        
        _simulateMortgageResult = LoggerMessage.Define<SimulateMortgage.SimulateMortgageResponse>(
            LogLevel.Debug,
            new EventId(LoggerEventIdCodes.OfferSimulateMortgageResult, nameof(OfferSimulateMortgageResult)),
            "Mortgage simulate results: {Results}");
    }

    public static void OfferSimulateMortgageStarted(this ILogger logger, SimulateMortgage.SimulateMortgageRequest request)
        => _simulateMortgageStarted(logger, request, null!);
    
    public static void OfferSimulateMortgageResult(this ILogger logger, SimulateMortgage.SimulateMortgageResponse result)
        => _simulateMortgageResult(logger, result, null!);
}

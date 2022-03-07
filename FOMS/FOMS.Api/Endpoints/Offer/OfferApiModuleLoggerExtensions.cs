namespace FOMS.Api.Endpoints.Offer;

internal static class OfferApiModuleLoggerExtensions
{
    private static readonly Action<ILogger, string, Exception> _simulateMortgageStarted;
    private static readonly Action<ILogger, SimulateMortgage.SimulateMortgageResponse, Exception> _simulateMortgageResult;

    static OfferApiModuleLoggerExtensions()
    {
        _simulateMortgageStarted = LoggerMessage.Define<string>(
            LogLevel.Debug,
            new EventId(LoggerEventIdCodes.Endpoints_Offer_SimulateMortgageStarted, nameof(OfferSimulateMortgageStarted)),
            "Simulation started with {Request}");
        
        _simulateMortgageResult = LoggerMessage.Define<SimulateMortgage.SimulateMortgageResponse>(
            LogLevel.Debug,
            new EventId(LoggerEventIdCodes.Endpoints_Offer_SimulateMortgageResult, nameof(OfferSimulateMortgageResult)),
            "Mortgage simulate results: {Results}");
    }

    public static void OfferSimulateMortgageStarted(this ILogger logger, SimulateMortgage.SimulateMortgageRequest request)
        => _simulateMortgageStarted(logger, System.Text.Json.JsonSerializer.Serialize(request), null!);
    
    public static void OfferSimulateMortgageResult(this ILogger logger, SimulateMortgage.SimulateMortgageResponse result)
        => _simulateMortgageResult(logger, result, null!);
}

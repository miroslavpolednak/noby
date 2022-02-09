namespace FOMS.Api.Endpoints.Offer;

internal static class OfferApiModuleLoggerExtensions
{
    private static readonly Action<ILogger, Dto.SimulateMortgageRequest, Exception> _simulateMortgageStarted;
    private static readonly Action<ILogger, Dto.SimulateMortgageResponse, Exception> _simulateMortgageResult;

    static OfferApiModuleLoggerExtensions()
    {
        _simulateMortgageStarted = LoggerMessage.Define<Dto.SimulateMortgageRequest>(
            LogLevel.Debug,
            new EventId(90101, nameof(SimulateMortgageStarted)),
            "Case {CaseId} created");
        
        _simulateMortgageResult = LoggerMessage.Define<Dto.SimulateMortgageResponse>(
            LogLevel.Debug,
            new EventId(90102, nameof(SimulateMortgageResult)),
            "Mortgage simulate results: {Results}");
    }

    public static void SimulateMortgageStarted(this ILogger logger, Dto.SimulateMortgageRequest request)
        => _simulateMortgageStarted(logger, request, null!);
    
    public static void SimulateMortgageResult(this ILogger logger, Dto.SimulateMortgageResponse result)
        => _simulateMortgageResult(logger, result, null!);
}

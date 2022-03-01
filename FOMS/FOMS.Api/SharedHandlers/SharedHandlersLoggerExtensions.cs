namespace FOMS.Api.SharedHandlers;

internal static class SharedHandlersLoggingExtensions
{
    private static readonly Action<ILogger, Requests.SharedCreateCaseRequest, Exception> _sharedCreateCaseStarted;
    private static readonly Action<ILogger, Requests.SharedCreateSalesArrangementRequest, Exception> _sharedCreateSalesArrangementStarted;
    
    static SharedHandlersLoggingExtensions()
    {
        _sharedCreateCaseStarted = LoggerMessage.Define<Requests.SharedCreateCaseRequest>(
            LogLevel.Debug,
            new EventId(LoggerEventIdCodes.Endpoints_Shared_CreateCaseStarted, nameof(SharedCreateCaseStarted)),
            "Try to create new Case with {Request}");
        
        _sharedCreateSalesArrangementStarted = LoggerMessage.Define<Requests.SharedCreateSalesArrangementRequest>(
            LogLevel.Debug,
            new EventId(LoggerEventIdCodes.Endpoints_Shared_CreateSalesArrangementStarted, nameof(SharedCreateSalesArrangementStarted)),
            "Try to create new SalesArrangement with {Request}");
    }

    public static void SharedCreateCaseStarted(this ILogger logger, Requests.SharedCreateCaseRequest request)
        => _sharedCreateCaseStarted(logger, request, null!);
    
    public static void SharedCreateSalesArrangementStarted(this ILogger logger, Requests.SharedCreateSalesArrangementRequest request)
        => _sharedCreateSalesArrangementStarted(logger, request, null!);
}
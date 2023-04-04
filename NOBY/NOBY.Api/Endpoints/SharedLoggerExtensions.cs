namespace NOBY.Api.Endpoints;

internal static class SharedLoggingExtensions
{
    private static readonly Action<ILogger, int, Exception> _sharedCreateCaseStarted;
    private static readonly Action<ILogger, int, long, int, Exception> _sharedCreateSalesArrangementStarted;
    
    static SharedLoggingExtensions()
    {
        _sharedCreateCaseStarted = LoggerMessage.Define<int>(
            LogLevel.Debug,
            new EventId(Infrastructure.LoggerEventIdCodes.EndpointsSharedCreateCaseStarted, nameof(SharedCreateCaseStarted)),
            "Try to create new Case from Offer {OfferId}");
        
        _sharedCreateSalesArrangementStarted = LoggerMessage.Define<int, long, int>(
            LogLevel.Debug,
            new EventId(Infrastructure.LoggerEventIdCodes.EndpointsSharedCreateSalesArrangementStarted, nameof(SharedCreateSalesArrangementStarted)),
            "Try to create new SalesArrangement of type {SalesArrangementTypeId} for Case {CaseId} from Offer {OfferId}");
    }

    public static void SharedCreateCaseStarted(this ILogger logger, int offerId)
        => _sharedCreateCaseStarted(logger, offerId, null!);
    
    public static void SharedCreateSalesArrangementStarted(this ILogger logger, int salesArrangementTypeId, long caseId, int offerId)
        => _sharedCreateSalesArrangementStarted(logger, salesArrangementTypeId, caseId, offerId, null!);
}
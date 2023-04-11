namespace DomainServices.CaseService.Api;

internal static class LoggerExtensions
{
    private static readonly Action<ILogger, long, Exception> _newCaseIdCreated;
    private static readonly Action<ILogger, CIS.Core.Types.Paginable, Exception> _searchCasesStart;
    private static readonly Action<ILogger, long, int, Exception> _updateCaseStateStart;
    private static readonly Action<ILogger, long, Exception> _caseStateChangedFailed;
    private static readonly Action<ILogger, int, long, Exception> _queueRequestIdSaved;
    private static readonly Action<ILogger, long, int, Exception> _starbuildStateUpdateFailed;

    static LoggerExtensions()
    {
        _newCaseIdCreated = LoggerMessage.Define<long>(
            LogLevel.Debug,
            new EventId(LoggerEventIdCodes.NewCaseIdCreated, nameof(NewCaseIdCreated)),
            "Case {CaseId} created");

        _searchCasesStart = LoggerMessage.Define<CIS.Core.Types.Paginable>(
            LogLevel.Debug,
            new EventId(LoggerEventIdCodes.SearchCasesStart, nameof(SearchCasesStart)),
            "Request in SearchCases started with {Pagination}");

        _updateCaseStateStart = LoggerMessage.Define<long, int>(
            LogLevel.Debug,
            new EventId(LoggerEventIdCodes.UpdateCaseStateStart, nameof(UpdateCaseStateStart)),
            "Update Case #{CaseId} state to {State}");

        _caseStateChangedFailed = LoggerMessage.Define<long>(
            LogLevel.Warning,
            new EventId(LoggerEventIdCodes.UpdateCaseStateStart, nameof(CaseStateChangedFailed)),
            "CaseStateChanged failed for {CaseId}");

        _queueRequestIdSaved = LoggerMessage.Define<int, long>(
            LogLevel.Information,
            new EventId(LoggerEventIdCodes.QueueRequestIdSaved, nameof(QueueRequestIdSaved)),
            "Saved RequestId {RequestId} for Case {CaseId}");

        _starbuildStateUpdateFailed = LoggerMessage.Define<long, int>(
            LogLevel.Warning,
            new EventId(LoggerEventIdCodes.StarbuildStateUpdateFailed, nameof(StarbuildStateUpdateFailed)),
            "Case state failed in Starbuild for {CaseId} with state {StateId}");
    }

    public static void NewCaseIdCreated(this ILogger logger, long caseId)
        => _newCaseIdCreated(logger, caseId, null!);

    public static void SearchCasesStart(this ILogger logger, CIS.Core.Types.Paginable pagination)
        => _searchCasesStart(logger, pagination, null!);

    public static void UpdateCaseStateStart(this ILogger logger, long caseId, int state)
        => _updateCaseStateStart(logger, caseId, state, null!);

    public static void CaseStateChangedFailed(this ILogger logger, long caseId, Exception ex)
        => _caseStateChangedFailed(logger, caseId, ex);

    public static void QueueRequestIdSaved(this ILogger logger, int requestId, long caseId)
        => _queueRequestIdSaved(logger, requestId, caseId, null!);

    public static void StarbuildStateUpdateFailed(this ILogger logger, long caseId, int stateId)
        => _starbuildStateUpdateFailed(logger, caseId, stateId, null!);
}

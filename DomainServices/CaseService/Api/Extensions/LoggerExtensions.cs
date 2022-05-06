namespace DomainServices.CaseService.Api;

internal static class LoggerExtensions
{
    private static readonly Action<ILogger, long, Exception> _newCaseIdCreated;
    private static readonly Action<ILogger, CIS.Core.Types.Paginable, Exception> _searchCasesStart;
    private static readonly Action<ILogger, long, int, Exception> _updateCaseStateStart;

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
    }

    public static void NewCaseIdCreated(this ILogger logger, long caseId)
        => _newCaseIdCreated(logger, caseId, null!);

    public static void SearchCasesStart(this ILogger logger, CIS.Core.Types.Paginable pagination)
        => _searchCasesStart(logger, pagination, null!);

    public static void UpdateCaseStateStart(this ILogger logger, long caseId, int state)
        => _updateCaseStateStart(logger, caseId, state, null!);
}

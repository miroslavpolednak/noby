namespace DomainServices.CaseService.Api;

internal static class LoggerExtensions
{
    private static readonly Action<ILogger, long, Exception> _newCaseIdCreated;
    private static readonly Action<ILogger, int, long, Exception> _linkCaseToOwnerStart;
    private static readonly Action<ILogger, CIS.Core.Types.Paginable, Exception> _searchCasesStart;
    private static readonly Action<ILogger, long, int, Exception> _updateCaseStateStart;

    static LoggerExtensions()
    {
        _newCaseIdCreated = LoggerMessage.Define<long>(
            LogLevel.Debug,
            new EventId(LoggerEventIdCodes.NewCaseIdCreated, nameof(NewCaseIdCreated)),
            "Case {CaseId} created");

        _linkCaseToOwnerStart = LoggerMessage.Define<int, long>(
            LogLevel.Debug,
            new EventId(LoggerEventIdCodes.LinkCaseToOwnerStart, nameof(LinkCaseToOwnerStart)),
            "Link Owner #{UserId} to Case #{CaseId}");

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

    public static void LinkCaseToOwnerStart(this ILogger logger, int caseOwnerUserId, long caseId)
        => _linkCaseToOwnerStart(logger, caseOwnerUserId, caseId, null!);

    public static void SearchCasesStart(this ILogger logger, CIS.Core.Types.Paginable pagination)
        => _searchCasesStart(logger, pagination, null!);

    public static void UpdateCaseStateStart(this ILogger logger, long caseId, int state)
        => _updateCaseStateStart(logger, caseId, state, null!);
}

using CIS.Core.Types;

namespace FOMS.Api.Endpoints.Case;

internal static class CaseLoggerExtensions
{
    private static readonly Action<ILogger, Paginable?, Exception> _searchPaginableSettings;

    static CaseLoggerExtensions()
    {
        _searchPaginableSettings = LoggerMessage.Define<Paginable?>(
            LogLevel.Debug,
            new EventId(LoggerEventIdCodes.Endpoints_Case_SearchPaginableSettings, nameof(SearchPaginableSettings)),
            "Search pagination set to {Pagination}");
    }

    public static void SearchPaginableSettings(this ILogger logger, Paginable? paginable)
        => _searchPaginableSettings(logger, paginable, null!);
}
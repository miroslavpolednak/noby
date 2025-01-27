﻿using CIS.Core.Types;

namespace NOBY.Api.Endpoints.Cases;

internal static class CasesLoggerExtensions
{
    private static readonly Action<ILogger, Paginable?, Exception> _searchPaginableSettings;

    static CasesLoggerExtensions()
    {
        _searchPaginableSettings = LoggerMessage.Define<Paginable?>(
            LogLevel.Debug,
            new EventId(Infrastructure.LoggerEventIdCodes.EndpointsCaseSearchPaginableSettings, nameof(SearchPaginableSettings)),
            "Search pagination set to {Pagination}");
    }

    public static void SearchPaginableSettings(this ILogger logger, Paginable? paginable)
        => _searchPaginableSettings(logger, paginable, null!);
}
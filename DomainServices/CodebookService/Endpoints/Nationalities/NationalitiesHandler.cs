﻿using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.Nationalities;

namespace DomainServices.CodebookService.Endpoints.Nationalities;

public class NationalitiesHandler
    : IRequestHandler<NationalitiesRequest, List<GenericCodebookItem>>
{
    public async Task<List<GenericCodebookItem>> Handle(NationalitiesRequest request, CancellationToken cancellationToken)
    {
        return await FastMemoryCache.GetOrCreate<GenericCodebookItem>(nameof(NationalitiesHandler), async () => await _connectionProvider.ExecuteDapperRawSqlToList<GenericCodebookItem>(_sqlQuery, cancellationToken));
    }

    private const string _sqlQuery =
        "SELECT Id, NazevStatniPrislusnost 'Name', CAST(1 as bit) 'IsValid' FROM [cis].[Zeme] ORDER BY NazevStatniPrislusnost ASC";

    private readonly CIS.Core.Data.IConnectionProvider<IKonsdbDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<NationalitiesHandler> _logger;

    public NationalitiesHandler(
        CIS.Core.Data.IConnectionProvider<IKonsdbDapperConnectionProvider> connectionProvider,
        ILogger<NationalitiesHandler> logger)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }
}

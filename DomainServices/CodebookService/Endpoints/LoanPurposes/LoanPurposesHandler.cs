﻿using DomainServices.CodebookService.Contracts.Endpoints.LoanPurposes;

namespace DomainServices.CodebookService.Endpoints.LoanPurposes;

public class LoanPurposesHandler
    : IRequestHandler<LoanPurposesRequest, List<LoanPurposesItem>>
{
    public async Task<List<LoanPurposesItem>> Handle(LoanPurposesRequest request, CancellationToken cancellationToken)
    {
        return await FastMemoryCache.GetOrCreate<LoanPurposesItem>(nameof(LoanPurposesHandler), async () =>
        {
            var items = await _connectionProvider.ExecuteDapperRawSqlToList<LoanPurposesItemExt>(_sqlQuery, cancellationToken);

            return items.Select(i => new LoanPurposesItem
            {
                Id = i.Id,
                Name = i.Name,
                MandantId = i.MandantId,
                ProductTypeIds = i.ProductTypeId?.ParseIDs(),
                Order = i.Order,
                C4mId = i.C4mId,
                IsValid = i.IsValid,

            }).ToList();
        });
    }

    private class LoanPurposesItemExt : LoanPurposesItem
    {
        public string? ProductTypeId { get; set; }
    }

    const string _sqlQuery = @"SELECT KOD 'Id', TEXT 'Name', NULLIF(MANDANT, 0) 'MandantId', KOD_UVER 'ProductTypeId', PORADI 'Order', MAPOVANI_C4M 'C4mId', CASE WHEN SYSDATETIME() BETWEEN [DATUM_PLATNOSTI_OD] AND ISNULL([DATUM_PLATNOSTI_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsValid'
                                FROM SBR.CIS_UCEL_UVERU_INT1 ORDER BY KOD";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<LoanPurposesHandler> _logger;

    public LoanPurposesHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider,
        ILogger<LoanPurposesHandler> logger)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }
}
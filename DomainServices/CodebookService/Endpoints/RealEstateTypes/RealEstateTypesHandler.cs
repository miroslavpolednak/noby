using DomainServices.CodebookService.Contracts.Endpoints.RealEstateTypes;

namespace DomainServices.CodebookService.Endpoints.RealEstateTypes;

public class RealEstateTypesHandler
    : IRequestHandler<RealEstateTypesRequest, List<RealEstateTypeItem>>
{
    public async Task<List<RealEstateTypeItem>> Handle(RealEstateTypesRequest request, CancellationToken cancellationToken)
    {
        return await FastMemoryCache.GetOrCreate<RealEstateTypeItem>(nameof(RealEstateTypesHandler), async () => await _connectionProvider.ExecuteDapperRawSqlToList<RealEstateTypeItem>(_sqlQuery, cancellationToken));
    }

    private const string _sqlQuery =
        "SELECT KOD 'Id', POPIS 'Name', NULLIF(MANDANT, 0) 'MandantId', CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsValid', DEF 'IsDefault', PORADI 'ORDER' FROM [SBR].[CIS_TYPY_NEHNUTELNOSTI_UV] ORDER BY PORADI ASC";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<RealEstateTypesHandler> _logger;

    public RealEstateTypesHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider,
        ILogger<RealEstateTypesHandler> logger)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }

    private const string _cacheKey = "RealEstateTypes";
}

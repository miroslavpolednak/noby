using DomainServices.CodebookService.Contracts.Endpoints.RiskApplicationTypes;

namespace DomainServices.CodebookService.Endpoints.RiskApplicationTypes;

public class RiskApplicationTypesHandler
    : IRequestHandler<RiskApplicationTypesRequest, List<RiskApplicationTypeItem>>
{
    public async Task<List<RiskApplicationTypeItem>> Handle(RiskApplicationTypesRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await FastMemoryCache.GetOrCreate<RiskApplicationTypeItem>(nameof(RiskApplicationTypesHandler), async () =>
            {
                var result = await _connectionProvider.ExecuteDapperRawSqlToList<RiskApplicationTypeItem>(_sqlQuery, cancellationToken);
                result.ForEach(t =>
                {
                    if (!string.IsNullOrEmpty(t.MA))
                        t.MarketingActions = t.MA.Split(",").Select(t => Convert.ToInt32(t)).ToList();
                    if (!string.IsNullOrEmpty(t.ProductId))
                        t.ProductTypeId = t.ProductId.Split(",").Select(t => Convert.ToInt32(t)).ToList();
                });
                return result;
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    const string _sqlQuery = @"SELECT ID 'Id', MANDANT 'Mandant', UV_PRODUKT_ID 'ProductId', MA, DRUH_UVERU 'LoanKindId', CAST(LTV_OD as int) 'LtvFrom', CAST(LTV_DO as int) 'LtvTo', CLUSTER_CODE 'C4mAplCode', C4M_APL_TYPE_ID 'C4mAplTypeId', C4M_APL_TYPE_NAZEV 'Name', CASE WHEN SYSDATETIME() BETWEEN [DATUM_OD] AND ISNULL([DATUM_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsValid' 
FROM [SBR].CIS_APL_TYPE ORDER BY ID ASC";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<RiskApplicationTypesHandler> _logger;

    public RiskApplicationTypesHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider,
        ILogger<RiskApplicationTypesHandler> logger)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }
}

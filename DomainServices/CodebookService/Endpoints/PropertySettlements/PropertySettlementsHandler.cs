using Dapper;
using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.PropertySettlements;

namespace DomainServices.CodebookService.Endpoints.PropertySettlements;

public class PropertySettlementsHandler
    : IRequestHandler<PropertySettlementsRequest, List<PropertySettlementItem>>
{
    public async Task<List<PropertySettlementItem>> Handle(PropertySettlementsRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await FastMemoryCache.GetOrCreate<PropertySettlementItem>(nameof(PropertySettlementsHandler), async () =>
                await _connectionProvider.ExecuteDapperRawSqlToList<PropertySettlementItem>(_sqlQuery, cancellationToken)
            );
        }
        catch (Exception ex)
        {
            _logger.GeneralException(ex);
            throw;
        }
    }

    const string _sqlQuery = @"SELECT KOD 'Id', CODE 'Code', TEXT_CZE 'Name', TEXT_ENG 'NameEng', CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsValid' 
                               FROM [SBR].[CIS_VYPORADANI_MAJETKU] ORDER BY KOD ASC";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<PropertySettlementsHandler> _logger;

    public PropertySettlementsHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider, 
        ILogger<PropertySettlementsHandler> logger)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }
}

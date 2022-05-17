using DomainServices.CodebookService.Contracts.Endpoints.IncomeOtherTypes;

namespace DomainServices.CodebookService.Endpoints.IncomeOtherTypes;

public class IncomeOtherTypesHandler
    : IRequestHandler<IncomeOtherTypesRequest, List<IncomeOtherTypeItem>>
{
    public async Task<List<IncomeOtherTypeItem>> Handle(IncomeOtherTypesRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await FastMemoryCache.GetOrCreate<IncomeOtherTypeItem>(nameof(IncomeOtherTypesHandler), async () =>
                await _connectionProvider.ExecuteDapperRawSqlToList<IncomeOtherTypeItem>(_sqlQuery, cancellationToken)
            );
        }
        catch (Exception ex)
        {
            _logger.GeneralException(ex);
            throw;
        }
    }

    const string _sqlQuery = @"SELECT KOD 'Id', CODE 'Code', TEXT_CZE 'Name', CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsValid' 
                               FROM [SBR].[CIS_ZDROJ_PRIJMU_VEDLAJSI] ORDER BY KOD ASC";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<IncomeOtherTypesHandler> _logger;

    public IncomeOtherTypesHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider, 
        ILogger<IncomeOtherTypesHandler> logger)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }
}

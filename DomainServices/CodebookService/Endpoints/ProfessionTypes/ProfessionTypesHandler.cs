using DomainServices.CodebookService.Contracts.Endpoints.ProfessionTypes;

namespace DomainServices.CodebookService.Endpoints.ProfessionTypes;

public class ProfessionTypesHandler
    : IRequestHandler<ProfessionTypesRequest, List<ProfessionTypeItem>>
{
    public async Task<List<ProfessionTypeItem>> Handle(ProfessionTypesRequest request, CancellationToken cancellationToken)
    {
        return await FastMemoryCache.GetOrCreate<ProfessionTypeItem>(nameof(ProfessionTypesHandler), async () => await _connectionProvider.ExecuteDapperRawSqlToList<ProfessionTypeItem>(_sqlQuery, cancellationToken));
    }

    const string _sqlQuery = @"SELECT KOD 'Id',  ID_CM 'RdmCode', NAZEV_CM 'Name', CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsValid'
                               FROM [SBR].[CIS_POVOLANI] ORDER BY KOD ASC";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<ProfessionTypesHandler> _logger;

    public ProfessionTypesHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider, 
        ILogger<ProfessionTypesHandler> logger)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }
}

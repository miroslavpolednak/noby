using DomainServices.CodebookService.Endpoints.StatementSubscriptionTypes;
using DomainServices.CodebookService.Contracts.Endpoints.StatementFrequencies;

namespace DomainServices.CodebookService.Endpoints.StatementFrequencies;

public class StatementFrequenciesHandler
    : IRequestHandler<StatementFrequenciesRequest, List<StatementFrequencyItem>>
{
    public async Task<List<StatementFrequencyItem>> Handle(StatementFrequenciesRequest request, CancellationToken cancellationToken)
    {
        return await FastMemoryCache.GetOrCreate(nameof(StatementSubscriptionTypesHandler), async () => await _connectionProvider.ExecuteDapperRawSqlToList<StatementFrequencyItem>(_sqlQuery, cancellationToken));
    }

    private const string _sqlQuery =
        "SELECT KOD 'Id', CODE 'FrequencyCode', FREQ 'FrequencyValue', SORT 'Order', [TEXT] 'Name', CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsValid', DEF 'IsDefault' FROM [SBR].[CIS_HU_VYPIS_FREQ] ORDER BY SORT";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<StatementFrequenciesHandler> _logger;

    public StatementFrequenciesHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider,
        ILogger<StatementFrequenciesHandler> logger)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }
}

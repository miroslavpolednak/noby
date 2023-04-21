using DomainServices.CodebookService.Contracts.Endpoints;
using DomainServices.CodebookService.Contracts.Endpoints.StatementSubscriptionTypes;

namespace DomainServices.CodebookService.Endpoints.StatementSubscriptionTypes;

public class StatementSubscriptionTypesHandler
    : IRequestHandler<StatementSubscriptionTypesRequest, List<GenericCodebookItemWithCodeAndDefault>>
{
    public async Task<List<GenericCodebookItemWithCodeAndDefault>> Handle(StatementSubscriptionTypesRequest request, CancellationToken cancellationToken)
    {
        return await FastMemoryCache.GetOrCreate(nameof(StatementSubscriptionTypesHandler), async () => await _connectionProvider.ExecuteDapperRawSqlToList<GenericCodebookItemWithCodeAndDefault>(_sqlQuery, cancellationToken));
    }

    private const string _sqlQuery =
        "SELECT KOD 'Id', CODE 'Code', [TEXT] 'Name', CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsValid', DEF 'IsDefault' FROM [SBR].[CIS_HU_ZODB_VYPIS]";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<StatementSubscriptionTypesHandler> _logger;

    public StatementSubscriptionTypesHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider,
        ILogger<StatementSubscriptionTypesHandler> logger)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }
}

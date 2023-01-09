using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.IncomeForeignTypes;

namespace DomainServices.CodebookService.Endpoints.IncomeForeignTypes;

public class IncomeForeignTypesHandler
    : IRequestHandler<IncomeForeignTypesRequest, List<GenericCodebookItemWithCode>>
{
    public async Task<List<GenericCodebookItemWithCode>> Handle(IncomeForeignTypesRequest request, CancellationToken cancellationToken)
    {
        return await FastMemoryCache.GetOrCreate<GenericCodebookItemWithCode>(nameof(IncomeForeignTypesHandler), async () => await _connectionProvider.ExecuteDapperRawSqlToList<GenericCodebookItemWithCode>(_sqlQuery, cancellationToken));
    }

    const string _sqlQuery = @"SELECT KOD 'Id', CODE 'Code', TEXT 'Name', CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsValid' 
                               FROM [SBR].[CIS_PRIJEM_ZO_ZAHRANICIA] ORDER BY KOD ASC";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<IncomeForeignTypesHandler> _logger;

    public IncomeForeignTypesHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider, 
        ILogger<IncomeForeignTypesHandler> logger)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }
}

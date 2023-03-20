using DomainServices.CodebookService.Contracts.Endpoints.SalesArrangementTypes;

namespace DomainServices.CodebookService.Endpoints.SalesArrangementTypes;
public class SalesArrangementTypesHandler
    : IRequestHandler<SalesArrangementTypesRequest, List<SalesArrangementTypeItem>>
{
    public async Task<List<SalesArrangementTypeItem>> Handle(SalesArrangementTypesRequest request, CancellationToken cancellationToken)
    {
        return await FastMemoryCache.GetOrCreate<SalesArrangementTypeItem>(nameof(SalesArrangementTypesHandler), async () =>
            await _connectionProvider.ExecuteDapperRawSqlToList<SalesArrangementTypeItem>(_sqlQuery, cancellationToken)
        );
    }

    private const string _sqlQuery = "SELECT Id, Name, ProductTypeId, SalesArrangementCategory FROM [dbo].[SalesArrangementType]";

    private readonly CIS.Core.Data.IConnectionProvider _connectionProvider;

    public SalesArrangementTypesHandler(CIS.Core.Data.IConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }
}

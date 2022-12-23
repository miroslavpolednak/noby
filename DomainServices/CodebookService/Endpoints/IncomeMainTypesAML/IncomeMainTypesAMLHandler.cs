using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.IncomeMainTypesAML;

namespace DomainServices.CodebookService.Endpoints.IncomeMainTypesAML;

public class IncomeMainTypesAMLHandler
    : IRequestHandler<IncomeMainTypesAMLRequest, List<GenericCodebookItemWithRdmCode>>
{
    public async Task<List<GenericCodebookItemWithRdmCode>> Handle(IncomeMainTypesAMLRequest request, CancellationToken cancellationToken)
    {
        return await FastMemoryCache.GetOrCreate(nameof(IncomeMainTypesAMLHandler), async () =>
        {
            var ext = await _connectionProviderCodebooks.ExecuteDapperRawSqlToList<ExtensionMapper>(_sqlQueryExtension, cancellationToken);

            var sbr = await _connectionProvider.ExecuteDapperRawSqlToList<GenericCodebookItemWithRdmCode>(_sqlQuery, cancellationToken);
            sbr.ForEach(t => t.RdmCode = ext.FirstOrDefault(x => x.Id == t.Id)?.RdmCode ?? string.Empty);

            return sbr.ToList();
        });
    }

    private class ExtensionMapper
    {
        public int Id { get; set; }
        public string RdmCode { get; set; } = string.Empty;
    }

    const string _sqlQuery = @"SELECT A.KOD 'Id', A.NAZEV 'Name', CAST(1 as bit) 'IsValid' FROM [SBR].[CIS_AML_ZDROJ_PRIJMU_HLAVNI] A LEFT JOIN dbo. ORDER BY KOD ASC";
    const string _sqlQueryExtension = "SELECT Id, RdmCode FROM dbo.IncomeMainTypesAMLExtension";

    private readonly CIS.Core.Data.IConnectionProvider _connectionProviderCodebooks;
    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    
    public IncomeMainTypesAMLHandler(
        CIS.Core.Data.IConnectionProvider connectionProviderCodebooks,
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider)
    {
        _connectionProviderCodebooks = connectionProviderCodebooks;
        _connectionProvider = connectionProvider;
    }
}

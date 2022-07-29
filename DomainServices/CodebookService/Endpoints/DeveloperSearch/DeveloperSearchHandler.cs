using DomainServices.CodebookService.Contracts.Endpoints.DeveloperSearch;

namespace DomainServices.CodebookService.Endpoints.DeveloperSearch;

public class DeveloperSearchHandler
    : IRequestHandler<DeveloperSearchRequest, List<DeveloperSearchItem>>
{
    public async Task<List<DeveloperSearchItem>> Handle(DeveloperSearchRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Term))
            return new List<DeveloperSearchItem>();

        var entries = request.Term.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        var dbArgs = new Dapper.DynamicParameters();
        for (int i= 0; i < entries.Length; i++)
            dbArgs.Add($"term{i}", entries[i]);
        string sqlSearch = string.Join(" OR ", entries.Select((t, index) => $"(A.NAZEV LIKE '%'+@term{index}+'%' OR B.PROJEKT LIKE '%'+@term{index}+'%' OR A.ICO_RC=@term{index})"));

        return await _connectionProvider.ExecuteDapperRawSqlToList<DeveloperSearchItem>(_sqlFirstPart + sqlSearch + _sqlOrder, dbArgs, cancellationToken);
    }

    const string _sqlFirstPart = @"
SELECT A.DEVELOPER_ID 'DeveloperId', A.NAZEV 'DeveloperName', A.ICO_RC 'DeveloperCIN', B.DEVELOPER_PROJEKT_ID 'DeveloperProjectId', B.PROJEKT 'DeveloperProjectName'
FROM [SBR].[CIS_DEVELOPER] A
INNER JOIN [SBR].[CIS_DEVELOPER_PROJEKTY_SPV] B ON A.DEVELOPER_ID=B.DEVELOPER_ID
WHERE GETDATE() BETWEEN A.[PLATNOST_OD] AND ISNULL(A.[PLATNOST_DO], '9999-12-31') AND GETDATE() BETWEEN B.[PLATNOST_OD] AND ISNULL(B.[PLATNOST_DO], '9999-12-31') AND A.PRIZNAK_OK=1 AND (";

    const string _sqlOrder = @") ORDER BY A.NAZEV ASC, B.PROJEKT ASC";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;

    public DeveloperSearchHandler(CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }
}

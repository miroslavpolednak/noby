using DomainServices.CodebookService.Contracts.Endpoints.DeveloperSearch;

namespace DomainServices.CodebookService.Endpoints.DeveloperSearch;

public class DeveloperSearchHandler
    : IRequestHandler<DeveloperSearchRequest, List<DeveloperSearchItem>>
{
    public async Task<List<DeveloperSearchItem>> Handle(DeveloperSearchRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Term))
            return new List<DeveloperSearchItem>();

        var terms = request.Term.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        var termsValues = String.Join(",", terms.Select(t => $"('{t}')"));

        var developersAndProjectsQuery = developersAndProjectsSqlTemplate.Replace("<terms>", termsValues);
        var developersQuery = developersSqlTemplate.Replace("<terms>", termsValues);

        var developersAndProjects = await _connectionProvider.ExecuteDapperRawSqlToList<DeveloperSearchItem>(developersAndProjectsQuery, cancellationToken);
        var developers = await _connectionProvider.ExecuteDapperRawSqlToList<DeveloperSearchItem>(developersQuery, cancellationToken);

        return developersAndProjects.Concat(developers).ToList();
    }

    // WITH terms AS (SELECT * FROM (VALUES (@term0),(@term1)) T(term))
    const string developersAndProjectsSqlTemplate = @"
        WITH terms AS (SELECT * FROM (VALUES <terms>) T(term))
        SELECT DEVELOPER_ID 'DeveloperId', NAZEV 'DeveloperName', ICO_RC 'DeveloperCIN', DEVELOPER_PROJEKT_ID 'DeveloperProjectId', PROJEKT 'DeveloperProjectName'
        FROM (
	        SELECT A.DEVELOPER_ID, A.NAZEV, A.ICO_RC, B.DEVELOPER_PROJEKT_ID, B.PROJEKT,
	        (
		        SELECT SUM(rate) FROM(
			        SELECT CAST(CAST(CHARINDEX(term, ISNULL(A.NAZEV,'')) AS BIT) AS INT)*1.01 + CAST(CAST(CHARINDEX(term, ISNULL(B.PROJEKT,'')) AS BIT) AS INT) + CAST(CAST(CHARINDEX(term, ISNULL(A.ICO_RC,'')) AS BIT) AS INT) AS rate FROM terms
		        )r
	        ) AS RATE
	        FROM [SBR].[CIS_DEVELOPER] A
	        INNER JOIN [SBR].[CIS_DEVELOPER_PROJEKTY_SPV] B ON A.DEVELOPER_ID=B.DEVELOPER_ID
	        WHERE GETDATE() BETWEEN A.[PLATNOST_OD] AND ISNULL(A.[PLATNOST_DO], '9999-12-31') AND GETDATE() BETWEEN B.[PLATNOST_OD] AND ISNULL(B.[PLATNOST_DO], '9999-12-31') AND A.PRIZNAK_OK=1
        )s
        WHERE RATE > 0 
        ORDER BY RATE DESC, NAZEV ASC, PROJEKT ASC";

    private const string developersSqlTemplate = @"
        WITH terms AS (SELECT * FROM (VALUES <terms>) T(term))
        SELECT DEVELOPER_ID 'DeveloperId', NAZEV 'DeveloperName', ICO_RC 'DeveloperCIN', null 'DeveloperProjectId', null 'DeveloperProjectName'
        FROM (
            SELECT A.DEVELOPER_ID, A.NAZEV, A.ICO_RC,
            (
                SELECT SUM(rate) FROM(
                    SELECT CAST(CAST(CHARINDEX(term, ISNULL(A.NAZEV,'')) AS BIT) AS INT)*1.01 + CAST(CAST(CHARINDEX(term, ISNULL(A.ICO_RC,'')) AS BIT) AS INT) AS rate FROM terms
                )r
            ) AS RATE
            FROM [SBR].[CIS_DEVELOPER] A
            WHERE GETDATE() BETWEEN A.[PLATNOST_OD] AND ISNULL(A.[PLATNOST_DO], '9999-12-31') AND A.PRIZNAK_OK=1
        )s
        WHERE RATE > 0
        ORDER BY RATE DESC, NAZEV ASC";
    
    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;

    public DeveloperSearchHandler(CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }
}

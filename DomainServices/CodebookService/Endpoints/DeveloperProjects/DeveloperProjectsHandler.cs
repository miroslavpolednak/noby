using DomainServices.CodebookService.Contracts.Endpoints.DeveloperProjects;
using DomainServices.CodebookService.Contracts.Endpoints.GetDeveloperProject;

namespace DomainServices.CodebookService.Endpoints.DeveloperProjects;

public class DeveloperProjectsHandler
    : IRequestHandler<DeveloperProjectsRequest, List<DeveloperProjectItem>>
{
    public async Task<List<DeveloperProjectItem>> Handle(DeveloperProjectsRequest request, CancellationToken cancellationToken)
    {
        return await FastMemoryCache.GetOrCreate<DeveloperProjectItem>(nameof(DeveloperProjectsHandler), async () => await _connectionProvider.ExecuteDapperRawSqlToList<DeveloperProjectItem>(_sqlQuery, cancellationToken));
    }

    const string _sqlQuery = @"SELECT DEVELOPER_PROJEKT_ID 'Id', DEVELOPER_ID 'DeveloperId', PROJEKT 'Name', UPOZORNENI_PRO_KB 'WarningForKb', UPOZORNENI_PRO_MPSS 'WarningForMp', STRANKY_PROJEKTU 'Web', LOKALITA 'Place', HROMADNE_OCENENI 'MassEvaluation', DOPORUCENI 'Recommandation'
                               , CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsValid' 
                               FROM [SBR].[CIS_DEVELOPER_PROJEKTY_SPV] ORDER BY DEVELOPER_PROJEKT_ID ASC";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<DeveloperProjectsHandler> _logger;

    public DeveloperProjectsHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider, 
        ILogger<DeveloperProjectsHandler> logger)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }
}

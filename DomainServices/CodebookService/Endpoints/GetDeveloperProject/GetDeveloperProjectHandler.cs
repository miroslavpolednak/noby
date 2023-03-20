using DomainServices.CodebookService.Contracts.Endpoints.GetDeveloperProject;

namespace DomainServices.CodebookService.Endpoints.GetDeveloperProject;

public class GetDeveloperProjectHandler
    : IRequestHandler<GetDeveloperProjectRequest, DeveloperProjectItem>
{
    public async Task<DeveloperProjectItem> Handle(GetDeveloperProjectRequest request, CancellationToken cancellationToken)
    {
        return await _connectionProvider.ExecuteDapperRawSqlFirstOrDefault<DeveloperProjectItem>(_sqlQuery, new { id = request.DeveloperProjectId }, cancellationToken)
            ?? throw new CIS.Core.Exceptions.CisNotFoundException(20001, $"DeveloperProjectId {request.DeveloperProjectId} not found");
    }

    const string _sqlQuery = @"SELECT 
	DEVELOPER_PROJEKT_ID 'Id', 
	DEVELOPER_ID 'DeveloperId', 
	PROJEKT 'Name', 
	UPOZORNENI_PRO_KB 'WarningForKb', 
	UPOZORNENI_PRO_MPSS 'WarningForMp', 
	STRANKY_PROJEKTU 'Web', 
	LOKALITA 'Place', 
	HROMADNE_OCENENI 'MassEvaluation', 
	DOPORUCENI 'Recommandation', 
	CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsValid' 
FROM [SBR].[CIS_DEVELOPER_PROJEKTY_SPV]
WHERE DEVELOPER_PROJEKT_ID=@id";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;

    public GetDeveloperProjectHandler(CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }
}

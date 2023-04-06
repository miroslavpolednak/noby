namespace DomainServices.CodebookService.Clients;

public partial interface ICodebookServiceClients
{
    Task<List<Contracts.Endpoints.DeveloperSearch.DeveloperSearchItem>> DeveloperSearch(string term, CancellationToken cancellationToken = default(CancellationToken));

    Task<Contracts.Endpoints.GetDeveloper.DeveloperItem> GetDeveloper(int developerId, CancellationToken cancellationToken = default(CancellationToken));

    Task<Contracts.Endpoints.GetDeveloperProject.DeveloperProjectItem> GetDeveloperProject(int developerId, int developerProjectId, CancellationToken cancellationToken = default(CancellationToken));

    Task<Contracts.Endpoints.GetOperator.GetOperatorItem> GetOperator(string login, CancellationToken cancellationToken = default(CancellationToken));
}

using DomainServices.CodebookService.Contracts.v1;

namespace DomainServices.CodebookService.Clients;

public partial interface ICodebookServiceClient
{
    Task<GetOperatorResponse> GetOperator(string performerLogin, CancellationToken cancellationToken = default(CancellationToken));

    Task<GetDeveloperResponse> GetDeveloper(int developerId, CancellationToken cancellationToken = default(CancellationToken));

    Task<GetDeveloperProjectResponse> GetDeveloperProject(int developerId, int developerProjectId, CancellationToken cancellationToken = default(CancellationToken));

    Task<List<DeveloperSearchResponse.Types.DeveloperSearchItem>> DeveloperSearch(string term, CancellationToken cancellationToken = default(CancellationToken));
}

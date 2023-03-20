using Azure.Core;
using DomainServices.CodebookService.Contracts;

namespace DomainServices.CodebookService.Clients;

internal partial class CodebookService : ICodebookServiceClients
{
    private readonly ICodebookService _codebookService;
    private readonly ClientsMemoryCache _cache;

    public async Task<List<Contracts.Endpoints.DeveloperSearch.DeveloperSearchItem>> DeveloperSearch(string term, CancellationToken cancellationToken = default(CancellationToken))
        => await _codebookService.DeveloperSearch(new Contracts.Endpoints.DeveloperSearch.DeveloperSearchRequest
        {
            Term = term
        }, cancellationToken);

    public async Task<Contracts.Endpoints.GetDeveloper.DeveloperItem> GetDeveloper(int developerId, CancellationToken cancellationToken = default(CancellationToken))
        => await _codebookService.GetDeveloper(new Contracts.Endpoints.GetDeveloper.GetDeveloperRequest
        {
            DeveloperId = developerId
        }, cancellationToken);

    public async Task<Contracts.Endpoints.GetDeveloperProject.DeveloperProjectItem> GetDeveloperProject(int developerProjectId, CancellationToken cancellationToken = default(CancellationToken))
        => await _codebookService.GetDeveloperProject(new Contracts.Endpoints.GetDeveloperProject.GetDeveloperProjectRequest
        {
            DeveloperProjectId = developerProjectId
        }, cancellationToken);

    public CodebookService(ICodebookService codebookService, ClientsMemoryCache cache)
    {
        _cache = cache;
        _codebookService = codebookService;
    }
}

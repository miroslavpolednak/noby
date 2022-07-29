using DomainServices.CodebookService.Contracts;

namespace DomainServices.CodebookService.Abstraction;

internal partial class CodebookService : ICodebookServiceAbstraction
{
    private readonly ICodebookService _codebookService;
    private readonly AbstractionMemoryCache _cache;

    public async Task<List<Contracts.Endpoints.DeveloperSearch.DeveloperSearchItem>> DeveloperSearch(string term, CancellationToken cancellationToken = default(CancellationToken))
        => await _codebookService.DeveloperSearch(new Contracts.Endpoints.DeveloperSearch.DeveloperSearchRequest
        {
            Term = term
        }, cancellationToken);

    public CodebookService(ICodebookService codebookService, AbstractionMemoryCache cache)
    {
        _cache = cache;
        _codebookService = codebookService;
    }
}

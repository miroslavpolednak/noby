namespace DomainServices.CodebookService.Abstraction;

public partial interface ICodebookServiceAbstraction
{
    Task<List<Contracts.Endpoints.DeveloperSearch.DeveloperSearchItem>> DeveloperSearch(string term, CancellationToken cancellationToken = default(CancellationToken));
}

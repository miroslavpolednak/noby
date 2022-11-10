namespace DomainServices.CodebookService.Clients;

public partial interface ICodebookServiceClients
{
    Task<List<Contracts.Endpoints.DeveloperSearch.DeveloperSearchItem>> DeveloperSearch(string term, CancellationToken cancellationToken = default(CancellationToken));
}

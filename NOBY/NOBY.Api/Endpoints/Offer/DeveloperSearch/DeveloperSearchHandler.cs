using CIS.Core.Types;

namespace NOBY.Api.Endpoints.Offer.DeveloperSearch;

internal sealed class DeveloperSearchHandler(DomainServices.CodebookService.Clients.ICodebookServiceClient _codebookService)
        : IRequestHandler<OfferDeveloperSearchRequest, OfferDeveloperSearchResponse>
{
    public async Task<OfferDeveloperSearchResponse> Handle(OfferDeveloperSearchRequest request, CancellationToken cancellationToken)
    {
        // vytvorit informaci o strankovani / razeni
        var paginable = Paginable
            .FromRequest(request.Pagination);

        var result = await _codebookService.DeveloperSearch(request.SearchText!, cancellationToken);

        var rows = result
            .Skip(paginable.RecordOffset * paginable.PageSize)
            .Take(paginable.PageSize)
            .ToList();

        return new OfferDeveloperSearchResponse
        {
            Rows = rows.Select(t => new CodebooksDeveloperSearchItem
            {
                DeveloperCIN = t.DeveloperCIN,
                DeveloperId = t.DeveloperId,
                DeveloperName = t.DeveloperName,
                DeveloperProjectId = t.DeveloperProjectId,
                DeveloperProjectName = t.DeveloperProjectName
            }).ToList(),
            Pagination = new(request.Pagination as IPaginableRequest ?? paginable, result.Count)
        };
    }
}

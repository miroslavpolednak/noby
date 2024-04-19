using CIS.Core.Types;
using CIS.Infrastructure.WebApi.Types;

namespace NOBY.Api.Endpoints.Offer.DeveloperSearch;

internal sealed class DeveloperSearchHandler(DomainServices.CodebookService.Clients.ICodebookServiceClient _codebookService)
        : IRequestHandler<DeveloperSearchRequest, DeveloperSearchResponse>
{
    public async Task<DeveloperSearchResponse> Handle(DeveloperSearchRequest request, CancellationToken cancellationToken)
    {
        // vytvorit informaci o strankovani / razeni
        var paginable = Paginable
            .FromRequest(request.Pagination);

        var result = await _codebookService.DeveloperSearch(request.SearchText!, cancellationToken);

        var rows = result
            .Skip(paginable.RecordOffset * paginable.PageSize)
            .Take(paginable.PageSize)
            .ToList();

        return new DeveloperSearchResponse
        {
            Rows = rows,
            Pagination = new PaginationResponse(request.Pagination as IPaginableRequest ?? paginable, result.Count)
        };
    }
}

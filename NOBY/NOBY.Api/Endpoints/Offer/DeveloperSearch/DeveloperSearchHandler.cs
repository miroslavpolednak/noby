﻿using CIS.Core.Types;
using CIS.Infrastructure.WebApi.Types;

namespace NOBY.Api.Endpoints.Offer.DeveloperSearch;

internal sealed class DeveloperSearchHandler
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

    private readonly DomainServices.CodebookService.Clients.ICodebookServiceClients _codebookService;

    public DeveloperSearchHandler(DomainServices.CodebookService.Clients.ICodebookServiceClients codebookService)
    {
        _codebookService = codebookService;
    }
}
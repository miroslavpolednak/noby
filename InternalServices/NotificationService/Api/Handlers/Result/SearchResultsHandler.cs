using CIS.InternalServices.NotificationService.Api.Services.Repositories;
using CIS.InternalServices.NotificationService.Api.Services.Repositories.Mappers;
using CIS.InternalServices.NotificationService.Contracts.Result;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Handlers.Result;

public class SearchResultsHandler : IRequestHandler<SearchResultsRequest, SearchResultsResponse>
{
    private readonly NotificationRepository _repository;

    public SearchResultsHandler(NotificationRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<SearchResultsResponse> Handle(SearchResultsRequest request, CancellationToken cancellationToken)
    {
        var results = await _repository.SearchResultsBy(
            request.Identity,
            request.IdentityScheme,
            request.CustomId,
            request.DocumentId);

        return new SearchResultsResponse
        {
            Results = results.Map().ToList()
        };
    }
}
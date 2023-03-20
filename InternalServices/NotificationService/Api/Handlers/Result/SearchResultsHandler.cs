using CIS.InternalServices.NotificationService.Api.Services.Messaging.Producers.Infrastructure;
using CIS.InternalServices.NotificationService.Api.Services.Repositories;
using CIS.InternalServices.NotificationService.Api.Services.Repositories.Mappers;
using CIS.InternalServices.NotificationService.Contracts.Result;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Handlers.Result;

public class SearchResultsHandler : IRequestHandler<SearchResultsRequest, SearchResultsResponse>
{
    private readonly UserAdapterService _userAdapterService;
    private readonly NotificationRepository _repository;

    public SearchResultsHandler(
        UserAdapterService userAdapterService,
        NotificationRepository repository)
    {
        _userAdapterService = userAdapterService;
        _repository = repository;
    }
    
    public async Task<SearchResultsResponse> Handle(SearchResultsRequest request, CancellationToken cancellationToken)
    {
        _userAdapterService.CheckReadResultAccess();
        
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
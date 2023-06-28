using CIS.InternalServices.NotificationService.Api.Services.Repositories.Abstraction;
using CIS.InternalServices.NotificationService.Api.Services.Repositories.Mappers;
using CIS.InternalServices.NotificationService.Api.Services.User.Abstraction;
using CIS.InternalServices.NotificationService.Contracts.Result;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Handlers.Result;

public class SearchResultsHandler : IRequestHandler<SearchResultsRequest, SearchResultsResponse>
{
    private readonly IUserAdapterService _userAdapterService;
    private readonly INotificationRepository _repository;

    public SearchResultsHandler(
        IUserAdapterService userAdapterService,
        INotificationRepository repository)
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
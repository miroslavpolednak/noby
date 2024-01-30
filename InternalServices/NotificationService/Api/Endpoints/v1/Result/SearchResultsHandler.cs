using CIS.InternalServices.NotificationService.Api.Endpoints.v1;
using CIS.InternalServices.NotificationService.Api.Legacy;
using CIS.InternalServices.NotificationService.Api.Services.User.Abstraction;
using CIS.InternalServices.NotificationService.LegacyContracts.Result;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v1.Result;

internal class SearchResultsHandler : IRequestHandler<SearchResultsRequest, SearchResultsResponse>
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
            request.CaseId,
            request.CustomId,
            request.DocumentId);

        return new SearchResultsResponse
        {
            Results = results.Map().ToList()
        };
    }
}
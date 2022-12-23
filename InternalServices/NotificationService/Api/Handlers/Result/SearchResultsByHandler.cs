using CIS.InternalServices.NotificationService.Api.Services.Repositories;
using CIS.InternalServices.NotificationService.Api.Services.Repositories.Mappers;
using CIS.InternalServices.NotificationService.Contracts.Result;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Handlers.Result;

public class SearchResultsByHandler : IRequestHandler<ResultsSearchByRequest, ResultsSearchByResponse>
{
    private readonly NotificationRepository _repository;

    public SearchResultsByHandler(NotificationRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<ResultsSearchByResponse> Handle(ResultsSearchByRequest request, CancellationToken cancellationToken)
    {
        var results = await _repository.SearchResultsBy(
            request.Identity,
            request.IdentityScheme,
            request.CustomId,
            request.DocumentId);

        return new ResultsSearchByResponse
        {
            Results = results.Map().ToList()
        };
    }
}
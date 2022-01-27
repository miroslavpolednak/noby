using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api.Handlers;

internal class GetCaseCountsHandler
    : IRequestHandler<Dto.GetCaseCountsMediatrRequest, GetCaseCountsResponse>
{
    public async Task<GetCaseCountsResponse> Handle(Dto.GetCaseCountsMediatrRequest request, CancellationToken cancellation)
    {
        _logger.LogInformation("Get counts for {userId}", request.CaseOwnerUserId);

        // vytahnout data z DB
        var model = await _repository.GetCounts(request.CaseOwnerUserId, cancellation);

        var result = new GetCaseCountsResponse();
        result.CaseCounts.AddRange(model.Select(t => new GetCaseCountsResponse.Types.CaseCountsItem { Count = t.Count, State = t.State }).ToList());
        
        return result;
    }

    private readonly Repositories.CaseServiceRepository _repository;
    private readonly ILogger<CreateCaseHandler> _logger;

    public GetCaseCountsHandler(
        Repositories.CaseServiceRepository repository,
        ILogger<CreateCaseHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}

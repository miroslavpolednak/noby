using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api.Handlers;

internal class GetCaseCountsHandler
    : IRequestHandler<Dto.GetCaseCountsMediatrRequest, GetCaseCountsResponse>
{
    public async Task<GetCaseCountsResponse> Handle(Dto.GetCaseCountsMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStartedWithId(nameof(GetCaseCountsHandler), request.CaseOwnerUserId);
        
        // vytahnout data z DB
        var model = await _repository.GetCounts(request.CaseOwnerUserId, cancellation);

        var result = new GetCaseCountsResponse();
        result.CaseCounts.AddRange(model.Select(t => new GetCaseCountsResponse.Types.CaseCountsItem { Count = t.Count, State = t.State }).ToList());

        _logger.FoundItems(result.CaseCounts.Count);

        return result;
    }

    private readonly Repositories.CaseServiceRepository _repository;
    private readonly ILogger<GetCaseCountsHandler> _logger;
    private readonly CIS.Core.Security.IServiceUserAccessor _userAccessor;
    
    public GetCaseCountsHandler(
        CIS.Core.Security.IServiceUserAccessor userAccessor,
        Repositories.CaseServiceRepository repository,
        ILogger<GetCaseCountsHandler> logger)
    {
        _userAccessor = userAccessor;
        _repository = repository;
        _logger = logger;
    }
}

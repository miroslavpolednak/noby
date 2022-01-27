namespace FOMS.Api.Endpoints.Case.Handlers;

internal class GetTotalsByStatesHandler
    : IRequestHandler<Dto.GetTotalsByStatesRequest, List<Dto.GetTotalsByStatesResponse>>
{
    public async Task<List<Dto.GetTotalsByStatesResponse>> Handle(Dto.GetTotalsByStatesRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("GetTotalsByStates");

        // zavolat BE sluzbu
        var result = CIS.Core.Results.ServiceCallResult.Resolve<DomainServices.CaseService.Contracts.GetCaseCountsResponse>(await _caseService.GetCaseCounts(_userAccessor.User.Id, cancellationToken));

        return result.CaseCounts.Select(t => new Dto.GetTotalsByStatesResponse
        {
            State = t.State,
            Count = t.Count
        }).ToList();
    }

    private readonly ILogger<SearchHandler> _logger;
    private readonly CIS.Core.Security.ICurrentUserAccessor _userAccessor;
    private readonly DomainServices.CaseService.Abstraction.ICaseServiceAbstraction _caseService;

    public GetTotalsByStatesHandler(
        CIS.Core.Security.ICurrentUserAccessor userAccessor,
        ILogger<SearchHandler> logger,
        DomainServices.CaseService.Abstraction.ICaseServiceAbstraction caseService)
    {
        _userAccessor = userAccessor;
        _logger = logger;
        _caseService = caseService;
    }
}
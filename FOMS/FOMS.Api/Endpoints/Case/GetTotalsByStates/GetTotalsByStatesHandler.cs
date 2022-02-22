namespace FOMS.Api.Endpoints.Case.GetTotalsByStates;

internal class GetTotalsByStatesHandler
    : IRequestHandler<GetTotalsByStatesRequest, List<GetTotalsByStatesResponse>>
{
    public async Task<List<GetTotalsByStatesResponse>> Handle(GetTotalsByStatesRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("GetTotalsByStates");

        // zavolat BE sluzbu
        var result = ServiceCallResult.Resolve<DomainServices.CaseService.Contracts.GetCaseCountsResponse>(await _caseService.GetCaseCounts(_userAccessor.User.Id, cancellationToken));

        return result.CaseCounts.Select(t => new GetTotalsByStatesResponse
        {
            State = t.State,
            Count = t.Count
        }).ToList();
    }

    private readonly ILogger<GetTotalsByStatesHandler> _logger;
    private readonly CIS.Core.Security.ICurrentUserAccessor _userAccessor;
    private readonly DomainServices.CaseService.Abstraction.ICaseServiceAbstraction _caseService;

    public GetTotalsByStatesHandler(
        CIS.Core.Security.ICurrentUserAccessor userAccessor,
        ILogger<GetTotalsByStatesHandler> logger,
        DomainServices.CaseService.Abstraction.ICaseServiceAbstraction caseService)
    {
        _userAccessor = userAccessor;
        _logger = logger;
        _caseService = caseService;
    }
}
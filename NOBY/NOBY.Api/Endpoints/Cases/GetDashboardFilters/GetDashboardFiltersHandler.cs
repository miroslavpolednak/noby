namespace NOBY.Api.Endpoints.Cases.GetTotalsByStates;

internal class GetDashboardFiltersHandler
    : IRequestHandler<GetDashboardFiltersRequest, List<GetDashboardFiltersResponse>>
{
    public async Task<List<GetDashboardFiltersResponse>> Handle(GetDashboardFiltersRequest request, CancellationToken cancellationToken)
    {
        // zavolat BE sluzbu
        var result = await _caseService.GetCaseCounts(_userAccessor.User!.Id, cancellationToken);

        // rucne vytvorena kolekce podle Motalika
        return new List<GetDashboardFiltersResponse>
        {
            new GetDashboardFiltersResponse(1, "Žádosti o úvěr", result.Where(t => t.State == 1 || t.State == 2).Select(t => t.Count).Sum()),
            new GetDashboardFiltersResponse(2, "Podepisování smluv", result.Where(t => t.State == 3).Select(t => t.Count).Sum()),
            new GetDashboardFiltersResponse(3, "Čerpání", result.Where(t => t.State == 4).Select(t => t.Count).Sum()),
            new GetDashboardFiltersResponse(4, "Správa", result.Where(t => t.State == 5).Select(t => t.Count).Sum()),
            new GetDashboardFiltersResponse(5, "Vše", result.Select(t => t.Count).Sum())
        };
    }

    private readonly CIS.Core.Security.ICurrentUserAccessor _userAccessor;
    private readonly DomainServices.CaseService.Clients.ICaseServiceClient _caseService;

    public GetDashboardFiltersHandler(
        CIS.Core.Security.ICurrentUserAccessor userAccessor,
        DomainServices.CaseService.Clients.ICaseServiceClient caseService)
    {
        _userAccessor = userAccessor;
        _caseService = caseService;
    }
}
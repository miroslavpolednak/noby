namespace NOBY.Api.Endpoints.Cases.GetTotalsByStates;

internal sealed class GetDashboardFiltersHandler(
    CIS.Core.Security.ICurrentUserAccessor _userAccessor,
    DomainServices.CaseService.Clients.v1.ICaseServiceClient _caseService)
        : IRequestHandler<GetDashboardFiltersRequest, List<GetDashboardFiltersResponse>>
{
    public async Task<List<GetDashboardFiltersResponse>> Handle(GetDashboardFiltersRequest request, CancellationToken cancellationToken)
    {
        // zavolat BE sluzbu
        var result = await _caseService.GetCaseCounts(_userAccessor.User!.Id, cancellationToken);

        result.RemoveAll(x => x.State is (6 or 7 or 10) || x.State is 5 && !_userAccessor.HasPermission(UserPermissions.CASE_ViewAfterDrawing));

        // rucne vytvorena kolekce podle Motalika
        return
        [
            new(1, "Vše", result.Select(t => t.Count).Sum()),
            new(2, "Žádosti o úvěr", result.Where(t => t.State is 1 or 2 or 8).Select(t => t.Count).Sum()),
            new(3, "Podepisování smluv", result.Where(t => t.State == 3).Select(t => t.Count).Sum()),
            new(4, "Čerpání", result.Where(t => t.State == 4).Select(t => t.Count).Sum()),
            new(5, "Správa", result.Where(t => t.State == 5).Select(t => t.Count).Sum())
        ];
    }
}
namespace NOBY.Api.Endpoints.Cases.GetDashboardFilters;

internal sealed class GetDashboardFiltersHandler(
    CIS.Core.Security.ICurrentUserAccessor _userAccessor,
    DomainServices.CaseService.Clients.v1.ICaseServiceClient _caseService)
        : IRequestHandler<GetDashboardFiltersRequest, List<CasesGetDashboardFiltersResponseItem>>
{
    public async Task<List<CasesGetDashboardFiltersResponseItem>> Handle(GetDashboardFiltersRequest request, CancellationToken cancellationToken)
    {
        // zavolat BE sluzbu
        var result = await _caseService.GetCaseCounts(_userAccessor.User!.Id, cancellationToken);

        result.RemoveAll(x => x.State is (6 or 7 or 10) || x.State is 5 && !_userAccessor.HasPermission(UserPermissions.CASE_ViewAfterDrawing));

        // rucne vytvorena kolekce podle Motalika
        return
        [
            new CasesGetDashboardFiltersResponseItem
            {
                FilterId = 1,
                Text = "Vše",
                CaseCount = result.Select(t => t.Count).Sum()
            },
            new CasesGetDashboardFiltersResponseItem
            {
                FilterId = 2,
                Text = "Žádosti o úvěr",
                CaseCount = result.Where(t => t.State is 1 or 2 or 8).Select(t => t.Count).Sum()
            },
            new CasesGetDashboardFiltersResponseItem
            {
                FilterId = 3,
                Text = "Podepisování smluv",
                CaseCount = result.Where(t => t.State == 3).Select(t => t.Count).Sum()
            },
            new CasesGetDashboardFiltersResponseItem
            {
                FilterId = 4,
                Text = "Čerpání",
                CaseCount = result.Where(t => t.State == 4).Select(t => t.Count).Sum()
            },
            new CasesGetDashboardFiltersResponseItem
            {
                FilterId = 5,
                Text = "Správa",
                CaseCount = result.Where(t => t.State == 5).Select(t => t.Count).Sum()
            }
        ];
    }
}
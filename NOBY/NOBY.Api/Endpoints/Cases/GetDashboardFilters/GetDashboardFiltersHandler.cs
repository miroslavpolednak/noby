namespace NOBY.Api.Endpoints.Cases.GetDashboardFilters;

internal sealed class GetDashboardFiltersHandler(
    CIS.Core.Security.ICurrentUserAccessor _userAccessor,
    DomainServices.CaseService.Clients.v1.ICaseServiceClient _caseService)
        : IRequestHandler<GetDashboardFiltersRequest, List<CasesGetDashboardFiltersResponseItem>>
{
    public async Task<List<CasesGetDashboardFiltersResponseItem>> Handle(GetDashboardFiltersRequest request, CancellationToken cancellationToken)
    {
        bool hasPerm = _userAccessor.HasPermission(UserPermissions.CASE_ViewAfterDrawing);
        // zavolat BE sluzbu
        var result = await _caseService.GetCaseCounts(_userAccessor.User!.Id, hasPerm ? null : 90, cancellationToken);

        // rucne vytvorena kolekce podle Motalika
        return
        [
            new CasesGetDashboardFiltersResponseItem
            {
                FilterId = 1,
                Text = "Vše",
                CaseCount = result
                    .Where(t => (EnumCaseStates)t.State is not (EnumCaseStates.Finished or EnumCaseStates.Cancelled or EnumCaseStates.ToBeCancelledConfirmed))
                    .Select(t => !hasPerm && t.State == 5 ? t.CountLimited!.Value : t.CountTotal)
                    .Sum()
            },
            new CasesGetDashboardFiltersResponseItem
            {
                FilterId = 2,
                Text = "Žádosti o úvěr",
                CaseCount = result
                    .Where(t => (EnumCaseStates)t.State is EnumCaseStates.InProgress or EnumCaseStates.InApproval or EnumCaseStates.InApprovalConfirmed)
                    .Select(t => t.CountTotal)
                    .Sum()
            },
            new CasesGetDashboardFiltersResponseItem
            {
                FilterId = 3,
                Text = "Podepisování smluv",
                CaseCount = result
                    .Where(t => t.State == (int)EnumCaseStates.InSigning)
                    .Select(t => t.CountTotal)
                    .Sum()
            },
            new CasesGetDashboardFiltersResponseItem
            {
                FilterId = 4,
                Text = "Čerpání",
                CaseCount = result
                    .Where(t => t.State == (int)EnumCaseStates.InDisbursement)
                    .Select(t => t.CountTotal)
                    .Sum()
            },
            new CasesGetDashboardFiltersResponseItem
            {
                FilterId = 5,
                Text = "Správa",
                CaseCount = result
                    .Where(t => t.State == (int)EnumCaseStates.InAdministration)
                    .Select(t => hasPerm ? t.CountTotal : t.CountLimited!.Value)
                    .Sum()
            }
        ];
    }
}
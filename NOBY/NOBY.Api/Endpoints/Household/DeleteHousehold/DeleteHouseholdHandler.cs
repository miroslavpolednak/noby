using CIS.Foms.Enums;
using DomainServices.HouseholdService.Clients;
using DomainServices.SalesArrangementService.Clients;

namespace NOBY.Api.Endpoints.Household.DeleteHousehold;

internal sealed class DeleteHouseholdHandler
    : IRequestHandler<DeleteHouseholdRequest, int>
{
    public async Task<int> Handle(DeleteHouseholdRequest request, CancellationToken cancellationToken)
    {
        var household = await _householdService.GetHousehold(request.HouseholdId, cancellationToken);

        // smazat
        await _householdService.DeleteHousehold(request.HouseholdId, cancellationToken: cancellationToken);

        // HFICH-5233
        if (household.HouseholdTypeId == (int)HouseholdTypes.Codebtor)
        {
            await _salesArrangementService.SetFlowSwitches(household.SalesArrangementId, new()
            {
                new() {
                    FlowSwitchId = (int)FlowSwitches.Was3602CodebtorChangedAfterSigning,
                    Value = true
                }
            }, cancellationToken);
        }

        return request.HouseholdId;
    }

    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IHouseholdServiceClient _householdService;
    
    public DeleteHouseholdHandler(
        IHouseholdServiceClient householdService, 
        ISalesArrangementServiceClient salesArrangementService)
    {
        _salesArrangementService = salesArrangementService;
        _householdService = householdService;
    }
}

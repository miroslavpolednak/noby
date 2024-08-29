using DomainServices.HouseholdService.Clients.v1;
using _HO = DomainServices.HouseholdService.Contracts;

namespace NOBY.Api.Endpoints.Household.UpdateHousehold;

internal sealed class UpdateHouseholdHandler(IHouseholdServiceClient _householdService) 
    : IRequestHandler<HouseholdUpdateHouseholdRequest>
{
    public async Task Handle(HouseholdUpdateHouseholdRequest request, CancellationToken cancellationToken)
    {
        var householdInstance = await _householdService.GetHousehold(request.HouseholdId, cancellationToken);

        // update domacnosti
        var householdRequest = new _HO.UpdateHouseholdRequest
        {
            CustomerOnSAId1 = householdInstance.CustomerOnSAId1,
            CustomerOnSAId2 = householdInstance.CustomerOnSAId2,
            HouseholdId = request.HouseholdId,
            Data = request.Data.ToDomainServiceRequest(),
            Expenses = request.Expenses.ToDomainServiceRequest()
        };

        await _householdService.UpdateHousehold(householdRequest, cancellationToken);
    }
}

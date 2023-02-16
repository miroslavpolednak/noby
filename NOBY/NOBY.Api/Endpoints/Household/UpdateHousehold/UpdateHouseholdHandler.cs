using DomainServices.HouseholdService.Clients;
using _HO = DomainServices.HouseholdService.Contracts;

namespace NOBY.Api.Endpoints.Household.UpdateHousehold;

internal class UpdateHouseholdHandler
    : IRequestHandler<UpdateHouseholdRequest>
{
    public async Task Handle(UpdateHouseholdRequest request, CancellationToken cancellationToken)
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

    private readonly IHouseholdServiceClient _householdService;

    public UpdateHouseholdHandler(IHouseholdServiceClient householdService)
    {
        _householdService = householdService;
    }
}

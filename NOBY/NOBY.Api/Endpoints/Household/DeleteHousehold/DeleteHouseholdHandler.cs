using DomainServices.HouseholdService.Clients;

namespace NOBY.Api.Endpoints.Household.DeleteHousehold;

internal sealed class DeleteHouseholdHandler
    : IRequestHandler<DeleteHouseholdRequest, int>
{
    public async Task<int> Handle(DeleteHouseholdRequest request, CancellationToken cancellationToken)
    {
        await _householdService.DeleteHousehold(request.HouseholdId, cancellationToken: cancellationToken);

        return request.HouseholdId;
    }

    private readonly IHouseholdServiceClient _householdService;
    
    public DeleteHouseholdHandler(IHouseholdServiceClient householdService)
    {
        _householdService = householdService;
    }
}

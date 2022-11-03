using DomainServices.HouseholdService.Clients;

namespace NOBY.Api.Endpoints.Household.DeleteHousehold;

internal class DeleteHouseholdHandler
    : IRequestHandler<DeleteHouseholdRequest, int>
{
    public async Task<int> Handle(DeleteHouseholdRequest request, CancellationToken cancellationToken)
    {
        ServiceCallResult.Resolve(await _householdService.DeleteHousehold(request.HouseholdId, cancellationToken));

        return request.HouseholdId;
    }

    private readonly IHouseholdServiceClient _householdService;
    private readonly ILogger<DeleteHouseholdHandler> _logger;

    public DeleteHouseholdHandler(
        IHouseholdServiceClient householdService,
        ILogger<DeleteHouseholdHandler> logger)
    {
        _logger = logger;
        _householdService = householdService;
    }
}

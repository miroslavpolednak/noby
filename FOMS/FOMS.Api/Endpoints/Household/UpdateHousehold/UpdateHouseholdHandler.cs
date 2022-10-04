using DomainServices.HouseholdService.Clients;
using _HO = DomainServices.HouseholdService.Contracts;

namespace FOMS.Api.Endpoints.Household.UpdateHousehold;

internal class UpdateHouseholdHandler
    : AsyncRequestHandler<UpdateHouseholdRequest>
{
    protected override async Task Handle(UpdateHouseholdRequest request, CancellationToken cancellationToken)
    {
        var householdInstance = ServiceCallResult.ResolveAndThrowIfError<_HO.Household>(await _householdService.GetHousehold(request.HouseholdId, cancellationToken));

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
    private readonly ILogger<UpdateHouseholdHandler> _logger;

    public UpdateHouseholdHandler(
        IHouseholdServiceClient householdService,
        ILogger<UpdateHouseholdHandler> logger)
    {
        _logger = logger;
        _householdService = householdService;
    }
}

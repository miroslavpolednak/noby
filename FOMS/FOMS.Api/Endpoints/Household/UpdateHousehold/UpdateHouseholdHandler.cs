using DomainServices.SalesArrangementService.Abstraction;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.Household.UpdateHousehold;

internal class UpdateHouseholdHandler
    : IRequestHandler<UpdateHouseholdRequest, int>
{
    public async Task<int> Handle(UpdateHouseholdRequest request, CancellationToken cancellationToken)
    {
        _logger.RequestHandlerStartedWithId(nameof(UpdateHouseholdHandler), request.HouseholdId);

        // update domacnosti
        var householdRequest = new _SA.UpdateHouseholdRequest
        {
            HouseholdId = request.HouseholdId,
            Data = request.Data.ToDomainServiceRequest(),
            Expenses = request.Expenses.ToDomainServiceRequest()
        };
        await _householdService.UpdateHousehold(householdRequest, cancellationToken);

        return request.HouseholdId;
    }

    private readonly IHouseholdServiceAbstraction _householdService;
    private readonly ILogger<UpdateHouseholdHandler> _logger;

    public UpdateHouseholdHandler(
        IHouseholdServiceAbstraction householdService,
        ILogger<UpdateHouseholdHandler> logger)
    {
        _logger = logger;
        _householdService = householdService;
    }
}

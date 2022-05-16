﻿using DomainServices.SalesArrangementService.Abstraction;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.Household.UpdateHousehold;

internal class UpdateHouseholdHandler
    : AsyncRequestHandler<UpdateHouseholdRequest>
{
    protected override async Task Handle(UpdateHouseholdRequest request, CancellationToken cancellationToken)
    {
        // update domacnosti
        var householdRequest = new _SA.UpdateHouseholdRequest
        {
            HouseholdId = request.HouseholdId,
            Data = request.Data.ToDomainServiceRequest(),
            Expenses = request.Expenses.ToDomainServiceRequest()
        };

        await _householdService.UpdateHousehold(householdRequest, cancellationToken);
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

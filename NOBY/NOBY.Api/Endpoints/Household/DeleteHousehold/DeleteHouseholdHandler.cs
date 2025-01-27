﻿using DomainServices.HouseholdService.Clients.v1;
using DomainServices.ProductService.Clients;
using DomainServices.SalesArrangementService.Clients;

namespace NOBY.Api.Endpoints.Household.DeleteHousehold;

internal sealed class DeleteHouseholdHandler(
    IFlowSwitchManager _flowSwitchManager,
    IProductServiceClient _productService,
    ICustomerOnSAServiceClient _customerOnSAService,
    IHouseholdServiceClient _householdService)
        : IRequestHandler<DeleteHouseholdRequest, int>
{
    public async Task<int> Handle(DeleteHouseholdRequest request, CancellationToken cancellationToken)
    {
        var household = await _householdService.GetHousehold(request.HouseholdId, cancellationToken);

        // smazat vazbu klient-produkt
        await deleteRelationship(household.CustomerOnSAId1, household.CaseId, cancellationToken);
        await deleteRelationship(household.CustomerOnSAId2, household.CaseId, cancellationToken);

        // smazat
        await _householdService.DeleteHousehold(request.HouseholdId, cancellationToken: cancellationToken);

        _flowSwitchManager.AddFlowSwitch(FlowSwitches.ScoringPerformedAtleastOnce, false);

        // ulozit flow switches
        await _flowSwitchManager.SaveFlowSwitches(household.SalesArrangementId, cancellationToken);

        return request.HouseholdId;
    }

    private async Task deleteRelationship(int? customerOnSA, long caseId, CancellationToken cancellationToken)
    {
        if (!customerOnSA.HasValue) return;

        var customer = await _customerOnSAService.GetCustomer(customerOnSA.Value, cancellationToken);

        var partnerId = customer
            .CustomerIdentifiers
            .GetMpIdentityOrDefault()
            ?.IdentityId;

        if (partnerId.HasValue)
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _productService.DeleteContractRelationship(Convert.ToInt32(partnerId.Value), caseId, cancellationToken);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }
    }
}

using CIS.Foms.Enums;
using DomainServices.HouseholdService.Clients;
using DomainServices.HouseholdService.Contracts;
using DomainServices.ProductService.Clients;
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

        // smazat vazbu klient-produkt
        var customers = await _customerOnSAService.GetCustomerList(household.SalesArrangementId, cancellationToken);
        await deleteRelationship(household.CustomerOnSAId1, household.CaseId, customers, cancellationToken);
        await deleteRelationship(household.CustomerOnSAId2, household.CaseId, customers, cancellationToken);

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

    private async Task deleteRelationship(int? customerOnSA, long caseId, List<CustomerOnSA> customers, CancellationToken cancellationToken)
    {
        if (!customerOnSA.HasValue) return;

        var partnerId = customers
            .First(t => t.CustomerOnSAId == customerOnSA.Value)
            ?.CustomerIdentifiers
            .FirstOrDefault(t => t.IdentityScheme == CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes.Mp)
            ?.IdentityId;

        if (partnerId.HasValue)
        {
            await _productService.DeleteContractRelationship(Convert.ToInt32(partnerId.Value), caseId, cancellationToken);
        }
    }

    private readonly IProductServiceClient _productService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IHouseholdServiceClient _householdService;
    private readonly ICustomerOnSAServiceClient _customerOnSAService;
    
    public DeleteHouseholdHandler(
        IProductServiceClient productService,
        ICustomerOnSAServiceClient customerOnSAService,
        IHouseholdServiceClient householdService, 
        ISalesArrangementServiceClient salesArrangementService)
    {
        _productService = productService;
        _customerOnSAService = customerOnSAService;
        _salesArrangementService = salesArrangementService;
        _householdService = householdService;
    }
}

using DomainServices.CodebookService.Clients;
using DomainServices.HouseholdService.Clients.v1;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;
using _HO = DomainServices.HouseholdService.Contracts;

namespace NOBY.Api.Endpoints.Household.CreateHousehold;

internal sealed class CreateHouseholdHandler(
    IFlowSwitchManager _flowSwitchManager,
    ICustomerOnSAServiceClient _customerOnSAService,
    IHouseholdServiceClient _householdService,
    ICodebookServiceClient _codebookService,
    ISalesArrangementServiceClient _salesArrangementService)
        : IRequestHandler<HouseholdCreateHouseholdRequest, HouseholdInList>
{
    public async Task<HouseholdInList> Handle(HouseholdCreateHouseholdRequest request, CancellationToken cancellationToken)
    {
        var saInstance = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);
        if (!saInstance.IsProductSalesArrangement() && !request.HardCreate)
        {
            throw new NobyValidationException("SalesArrangementTypeId is Service SA");
        }

        // typ domacnosti
        var householdType = (await _codebookService.HouseholdTypes(cancellationToken))
            .FirstOrDefault(x => x.Id == request.HouseholdTypeId)
            ?? throw new NobyValidationException($"Household type {request.HouseholdTypeId} not found");

        // kontrola max mnozstvi domacnosti
        var householdList = await _householdService.GetHouseholdList(request.SalesArrangementId, cancellationToken);
        if (householdList.Count(t => t.HouseholdTypeId == request.HouseholdTypeId) >= householdType.MaxHouseholdsForSA)
        {
            throw new NobyValidationException("Maximum count for this household type already reached");
        }

        // vytvorit domacnost
        var requestModel = new _HO.CreateHouseholdRequest
        {
            SalesArrangementId = request.SalesArrangementId,
            HouseholdTypeId = request.HouseholdTypeId
        };
        int householdId = await _householdService.CreateHousehold(requestModel, cancellationToken);

        // vytvorit customera
        var customerResponse = await _customerOnSAService.CreateCustomer(new _HO.CreateCustomerRequest
        {
            SalesArrangementId = request.SalesArrangementId,
            CustomerRoleId = (int)(request.HouseholdTypeId == (int)HouseholdTypes.Main ? SharedTypes.Enums.EnumCustomerRoles.Debtor : SharedTypes.Enums.EnumCustomerRoles.Codebtor),
            Customer = new _HO.CustomerOnSABase()
        }, cancellationToken);

        // vlozit customera na household
        await _householdService.LinkCustomerOnSAToHousehold(householdId, customerResponse.CustomerOnSAId, null, cancellationToken);

        _flowSwitchManager.AddFlowSwitch(FlowSwitches.ScoringPerformedAtleastOnce, false);

        // HFICH-5233
        if (request.HouseholdTypeId == (int)HouseholdTypes.Codebtor)
        {
            _flowSwitchManager.AddFlowSwitch(FlowSwitches.Was3602CodebtorChangedAfterSigning, true);
        }

        // ulozit flow switches
        await _flowSwitchManager.SaveFlowSwitches(request.SalesArrangementId, cancellationToken);

        return new HouseholdInList
        {
            HouseholdId = householdId,
            HouseholdTypeId = request.HouseholdTypeId,
            HouseholdTypeName = householdType.Name
        };
    }
}

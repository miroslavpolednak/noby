﻿using CIS.Foms.Enums;
using DomainServices.CodebookService.Clients;
using DomainServices.HouseholdService.Clients;
using DomainServices.SalesArrangementService.Contracts;
using _HO = DomainServices.HouseholdService.Contracts;

namespace NOBY.Api.Endpoints.Household.CreateHousehold;

internal sealed class CreateHouseholdHandler
    : IRequestHandler<CreateHouseholdRequest, Dto.HouseholdInList>
{
    public async Task<Dto.HouseholdInList> Handle(CreateHouseholdRequest request, CancellationToken cancellationToken)
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
            CustomerRoleId = (int)(request.HouseholdTypeId == (int)HouseholdTypes.Main ? CustomerRoles.Debtor : CustomerRoles.Codebtor),
            Customer = new _HO.CustomerOnSABase()
        }, cancellationToken);

        // vlozit customera na household
        await _householdService.LinkCustomerOnSAToHousehold(householdId, customerResponse.CustomerOnSAId, null, cancellationToken);

        // HFICH-5233
        if (request.HouseholdTypeId == (int)HouseholdTypes.Codebtor)
        {
            await _salesArrangementService.SetFlowSwitches(request.SalesArrangementId, new()
            {
                new()
                { 
                    FlowSwitchId = (int)FlowSwitches.Was3602CodebtorChangedAfterSigning, 
                    Value = true 
                }
            }, cancellationToken);
        }

        return new Dto.HouseholdInList
        {
            HouseholdId = householdId,
            SalesArrangementId = saInstance.SalesArrangementId,
            CaseId = saInstance.CaseId,
            HouseholdTypeId = request.HouseholdTypeId,
            HouseholdTypeName = householdType.Name
        };
    }

    private readonly DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient _salesArrangementService;
    private readonly ICodebookServiceClient _codebookService;
    private readonly IHouseholdServiceClient _householdService;
    private readonly ICustomerOnSAServiceClient _customerOnSAService;

    public CreateHouseholdHandler(
        ICustomerOnSAServiceClient customerOnSAService,
        IHouseholdServiceClient householdService,
        ICodebookServiceClient codebookService,
        DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient salesArrangementService)
    {
        _codebookService = codebookService;
        _householdService = householdService;
        _customerOnSAService = customerOnSAService;
        _salesArrangementService = salesArrangementService;
    }
}

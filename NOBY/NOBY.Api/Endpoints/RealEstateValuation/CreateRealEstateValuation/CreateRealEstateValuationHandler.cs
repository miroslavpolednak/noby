﻿using CIS.Foms.Enums;
using DomainServices.CaseService.Clients;
using DomainServices.OfferService.Clients;
using DomainServices.RealEstateValuationService.Clients;
using DomainServices.SalesArrangementService.Clients;

namespace NOBY.Api.Endpoints.RealEstateValuation.CreateRealEstateValuation;

internal sealed class CreateRealEstateValuationHandler
    : IRequestHandler<CreateRealEstateValuationRequest, int>
{
    public async Task<int> Handle(CreateRealEstateValuationRequest request, CancellationToken cancellationToken)
    {
        var caseInstance = await _caseService.GetCaseDetail(request.CaseId, cancellationToken);

        var revRequest = new DomainServices.RealEstateValuationService.Contracts.CreateRealEstateValuationRequest
        {
            CaseId = request.CaseId,
            RealEstateTypeId = request.RealEstateTypeId,
            IsLoanRealEstate = request.IsLoanRealEstate,
            ValuationTypeId = DomainServices.RealEstateValuationService.Contracts.ValuationTypes.Unknown,
            DeveloperApplied = request.DeveloperApplied,
            ValuationStateId = (int)RealEstateValuationStates.Rozpracovano
        };

        if (caseInstance.State == (int)CaseStates.InProgress)
        {
            var saInstance = await _salesArrangementService.GetProductSalesArrangement(request.CaseId, cancellationToken);

            // kontrola HFICH-4168
            var flowSwitches = await _salesArrangementService.GetFlowSwitches(saInstance.SalesArrangementId, cancellationToken);
            if (!flowSwitches.Any(t => t.FlowSwitchId == (int)FlowSwitches.IsRealEstateValuationAllowed && t.Value))
            {
                throw new NobyValidationException("FlowSwitch IsRealEstateValuationAllowed is not set");
            }

            var developer = await _offerService.GetOfferDeveloper(saInstance.OfferId!.Value, cancellationToken);

            revRequest.DeveloperAllowed = developer.IsDeveloperAllowed && request.IsLoanRealEstate;
            
            if (request.DeveloperApplied && revRequest.DeveloperAllowed)
            {
                revRequest.ValuationStateId = 4;
            }
        }

        if (!revRequest.DeveloperAllowed && request.DeveloperApplied)
        {
            throw new CisAuthorizationException("Developer check failed");
        }

        return await _realEstateValuationService.CreateRealEstateValuation(revRequest, cancellationToken);
    }

    private readonly IOfferServiceClient _offerService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ICaseServiceClient _caseService;
    private readonly IRealEstateValuationServiceClient _realEstateValuationService;

    public CreateRealEstateValuationHandler(
        IOfferServiceClient offerService,
        ISalesArrangementServiceClient salesArrangementService,
        IRealEstateValuationServiceClient realEstateValuationService, 
        ICaseServiceClient caseService)
    {
        _offerService = offerService;
        _salesArrangementService = salesArrangementService;
        _realEstateValuationService = realEstateValuationService;
        _caseService = caseService;
    }
}

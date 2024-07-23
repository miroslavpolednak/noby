using DomainServices.CaseService.Clients.v1;
using DomainServices.CodebookService.Clients;
using DomainServices.OfferService.Clients.v1;
using DomainServices.RealEstateValuationService.Clients;
using DomainServices.SalesArrangementService.Clients;

namespace NOBY.Api.Endpoints.RealEstateValuation.CreateRealEstateValuation;

internal sealed class CreateRealEstateValuationHandler(
    ICodebookServiceClient _codebookService,
    IOfferServiceClient _offerService,
    ISalesArrangementServiceClient _salesArrangementService,
    IRealEstateValuationServiceClient _realEstateValuationService,
    ICaseServiceClient _caseService)
        : IRequestHandler<RealEstateValuationCreateRealEstateValuationRequest, int>
{
    public async Task<int> Handle(RealEstateValuationCreateRealEstateValuationRequest request, CancellationToken cancellationToken)
    {
        var isCollateral = (await _codebookService.RealEstateTypes(cancellationToken))
            .FirstOrDefault(t => t.Id == request.RealEstateTypeId)
            ?.Collateral ?? false;
        if (!isCollateral)
        {
            throw new NobyValidationException(90032);
        }

        var caseInstance = await _caseService.GetCaseDetail(request.CaseId, cancellationToken);

        var revRequest = new DomainServices.RealEstateValuationService.Contracts.CreateRealEstateValuationRequest
        {
            CaseId = request.CaseId,
            RealEstateTypeId = request.RealEstateTypeId,
            IsLoanRealEstate = request.IsLoanRealEstate,
            ValuationTypeId = DomainServices.RealEstateValuationService.Contracts.ValuationTypes.Unknown,
            DeveloperApplied = request.DeveloperApplied,
            ValuationStateId = (int)RealEstateValuationStates.Rozpracovano,
            IsOnlineDisqualified = false
        };

        if (caseInstance.State == (int)EnumCaseStates.InProgress)
        {
            var saInstance = (await _salesArrangementService.GetProductSalesArrangements(request.CaseId, cancellationToken)).First();

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
                revRequest.ValuationStateId = (int)RealEstateValuationStates.Dokonceno;
            }
        }

        if (!revRequest.DeveloperAllowed && request.DeveloperApplied)
        {
            throw new NobyValidationException(90032, "Developer check failed");
        }

        return await _realEstateValuationService.CreateRealEstateValuation(revRequest, cancellationToken);
    }
}

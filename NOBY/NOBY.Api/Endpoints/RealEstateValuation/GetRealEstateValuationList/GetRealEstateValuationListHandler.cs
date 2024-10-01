using DomainServices.CaseService.Clients.v1;
using DomainServices.CaseService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.OfferService.Clients.v1;
using DomainServices.RealEstateValuationService.Clients;
using DomainServices.SalesArrangementService.Clients;

namespace NOBY.Api.Endpoints.RealEstateValuation.GetRealEstateValuationList;

internal sealed class GetRealEstateValuationListHandler(
    IOfferServiceClient _offerService,
    ISalesArrangementServiceClient _salesArrangementService,
    ICodebookServiceClient _codebookService,
    IRealEstateValuationServiceClient _realEstateValuationService,
    ICaseServiceClient _caseService)
        : IRequestHandler<GetRealEstateValuationListRequest, List<RealEstateValuationSharedRealEstateValuationListItem>>
{
    public async Task<List<RealEstateValuationSharedRealEstateValuationListItem>> Handle(GetRealEstateValuationListRequest request, CancellationToken cancellationToken)
    {
        var caseInstance = await _caseService.GetCaseDetail(request.CaseId, cancellationToken);

        // dopocitana oceneni na zaklade dat v SA
        List<RealEstateValuationSharedRealEstateValuationListItem>? computedValuations = null;
        if (caseInstance.IsInState([EnumCaseStates.InProgress]))
        {
            computedValuations = await getComputedValuations(request.CaseId, cancellationToken);
        }

        // oceneni ulozena u nas v DB
        var existingValuations = await getExistingValuations(request.CaseId, cancellationToken);

        if (computedValuations?.Any() ?? false)
        {
            var arr = existingValuations.Where(t => t.IsLoanRealEstate).Select(t => t.RealEstateTypeId).ToArray();
            existingValuations.AddRange(computedValuations.Where(t => !arr.Contains(t.RealEstateTypeId)));
            return existingValuations;
        }
        else
        {
            return existingValuations;
        }
    }

    /// <summary>
    /// Ziskat dopocitane nemovitosti - tj. ty, ktere jsou zadane na Offer pro dany SA
    /// </summary>
    private async Task<List<RealEstateValuationSharedRealEstateValuationListItem>?> getComputedValuations(long caseId, CancellationToken cancellationToken)
    {
        var saId = (await _salesArrangementService.GetProductSalesArrangements(caseId, cancellationToken)).First().SalesArrangementId;
        var saInstance = await _salesArrangementService.GetSalesArrangement(saId, cancellationToken);

        if (saInstance.Mortgage is null)
        {
            throw new NobyValidationException("SA.Mortgage object is null");
        }

        var developer = await _offerService.GetOfferDeveloper(saInstance.OfferId!.Value, cancellationToken);

        var state = (await _codebookService.WorkflowTaskStatesNoby(cancellationToken))
            .First(t => t.Id == 6);

        return saInstance.Mortgage
            .LoanRealEstates?
            .Where(t => t.IsCollateral)
            .Select(t => new RealEstateValuationSharedRealEstateValuationListItem
            {
                CaseId = saInstance.CaseId,
                RealEstateTypeId = t.RealEstateTypeId,
                RealEstateTypeIcon = RealEstateValuationHelpers.GetRealEstateTypeIcon(t.RealEstateTypeId),
                ValuationStateId = state.Id,
                ValuationStateIndicator = (EnumStateIndicators)state.Indicator,
                ValuationStateName = state.Name,
                IsLoanRealEstate = true,
                DeveloperAllowed = developer.IsDeveloperAllowed
            })
            .ToList();
    }

    /// <summary>
    /// Ziskat realne ulozene nemovitosti, ktere mame u nas v DB
    /// </summary>
    private async Task<List<RealEstateValuationSharedRealEstateValuationListItem>> getExistingValuations(long caseId, CancellationToken cancellationToken)
    {
        var revList = await _realEstateValuationService.GetRealEstateValuationList(caseId, cancellationToken);
        var states = await _codebookService.WorkflowTaskStatesNoby(cancellationToken);
        var priceTypes = await _codebookService.RealEstatePriceTypes(cancellationToken);

        return revList.Select(t => {
            var state = states.First(x => x.Id == t.ValuationStateId);

            var model = new RealEstateValuationSharedRealEstateValuationListItem
            {
                RealEstateValuationId = t.RealEstateValuationId,
                OrderId = t.OrderId,
                CaseId = t.CaseId,
                RealEstateTypeId = t.RealEstateTypeId,
                RealEstateTypeIcon = RealEstateValuationHelpers.GetRealEstateTypeIcon(t.RealEstateTypeId),
                ValuationStateId = t.ValuationStateId,
                ValuationStateIndicator = (EnumStateIndicators)state.Indicator,
                ValuationStateName = state.Name,
                IsLoanRealEstate = t.IsLoanRealEstate,
                RealEstateStateId = t.RealEstateStateId,
                ValuationTypeId = (EnumRealEstateValuationTypes)t.ValuationTypeId,
                Address = t.Address,
                ValuationSentDate = t.ValuationSentDate,
                IsRevaluationRequired = t.IsRevaluationRequired,
                DeveloperAllowed = t.DeveloperAllowed,
                DeveloperApplied = t.DeveloperApplied,
                PossibleValuationTypeId = t.PossibleValuationTypeId?.Select(t => (EnumRealEstateValuationTypes)t).ToList(),
                Prices = t.Prices?.Select(x => new RealEstateValuationSharedRealEstateValuationListItemPriceDetail
                {
                    Price = x.Price,
                    PriceTypeName = priceTypes.FirstOrDefault(xx => xx.Code == x.PriceSourceType)?.Name ?? x.PriceSourceType
                }).ToList()
            };

            return model;
        }).ToList();
    }
}

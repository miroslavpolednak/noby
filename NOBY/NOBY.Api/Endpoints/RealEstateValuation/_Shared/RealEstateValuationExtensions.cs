using DomainServices.CodebookService.Contracts.v1;
using __Contracts = DomainServices.RealEstateValuationService.Contracts;

namespace NOBY.Api.Endpoints.RealEstateValuation.Shared;

internal static class RealEstateValuationExtensions
{
    public static RealEstateValuationListItem MapToApiResponse(this __Contracts.RealEstateValuationListItem dsValuationListItem, IList<WorkflowTaskStatesNobyResponse.Types.WorkflowTaskStatesNobyItem> states)
    {
        // tvrdi ze tam bude vzdy zaznam a v EA neni zadne osetreni...
        var state = states.First(x => x.Id == dsValuationListItem.ValuationStateId);

        return new RealEstateValuationListItem
        {
            RealEstateValuationId = dsValuationListItem.RealEstateValuationId,
            OrderId = dsValuationListItem.OrderId,
            CaseId = dsValuationListItem.CaseId,
            RealEstateTypeId = dsValuationListItem.RealEstateTypeId,
            RealEstateTypeIcon = __Contracts.Helpers.GetRealEstateTypeIcon(dsValuationListItem.RealEstateTypeId),
            ValuationStateId = dsValuationListItem.RealEstateValuationId,
            ValuationStateIndicator = (ValuationStateIndicators)state.Indicator,
            ValuationStateName = state.Name,
            IsLoanRealEstate = dsValuationListItem.IsLoanRealEstate,
            RealEstateStateId = (RealEstateStateIds)dsValuationListItem.RealEstateStateId.GetValueOrDefault(),
            ValuationTypeId = dsValuationListItem.ValuationTypeId,
            Address = dsValuationListItem.Address,
            ValuationSentDate = dsValuationListItem.ValuationSentDate,
            ValuationResultCurrentPrice = dsValuationListItem.ValuationResultCurrentPrice,
            ValuationResultFuturePrice = dsValuationListItem.ValuationResultFuturePrice,
            IsRevaluationRequired = dsValuationListItem.IsRevaluationRequired,
            DeveloperAllowed = dsValuationListItem.DeveloperAllowed,
            DeveloperApplied = dsValuationListItem.DeveloperApplied
        };
    }
}
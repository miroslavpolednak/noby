using System.Linq.Expressions;

namespace DomainServices.RealEstateValuationService.Api.Database;

internal static class Mappers
{
    public static Expression<Func<Entities.RealEstateValuation, Contracts.RealEstateValuationListItem>> RealEstateDetail()
    {
        return t => new Contracts.RealEstateValuationListItem
        {
            RealEstateTypeId = t.RealEstateTypeId,
            RealEstateSubtypeId = t.RealEstateSubtypeId,
            CaseId = t.CaseId,
            IsLoanRealEstate = t.IsLoanRealEstate,
            DeveloperApplied = t.DeveloperApplied,
            DeveloperAllowed = t.DeveloperAllowed,
            RealEstateValuationId = t.RealEstateValuationId,
            ValuationStateId = t.ValuationStateId,
            ValuationTypeId = (Contracts.ValuationTypes)t.ValuationTypeId,
            IsRevaluationRequired = t.IsRevaluationRequired,
            ValuationSentDate = t.ValuationSentDate,
            RealEstateStateId = t.RealEstateStateId,
            Address = t.Address,
            OrderId = t.OrderId,
            ValuationResultCurrentPrice = t.ValuationResultCurrentPrice,
            ValuationResultFuturePrice = t.ValuationResultFuturePrice
        };
    }
}

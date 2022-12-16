using DomainServices.SalesArrangementService.Contracts;

namespace CIS.InternalServices.DataAggregator.EasForms.FormData.ProductRequest;

internal class LoanRealEstate
{
    public required int RowNumber { get; init; }

    public required SalesArrangementParametersMortgage.Types.LoanRealEstate LoanRealEstateData { get; init; }
}
using DomainServices.SalesArrangementService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.EasForms.FormData.ProductRequest;

internal class LoanRealEstate
{
    public required int RowNumber { get; init; }

    public required SalesArrangementParametersMortgage.Types.LoanRealEstate LoanRealEstateData { get; init; }
}
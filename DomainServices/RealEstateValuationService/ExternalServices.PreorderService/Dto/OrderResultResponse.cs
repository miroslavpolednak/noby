namespace DomainServices.RealEstateValuationService.ExternalServices.PreorderService.Dto;

public sealed class OrderResultResponse
{
    public decimal? ValuationResultCurrentPrice { get; set; }
    public decimal? ValuationResultFuturePrice { get; set; }
}

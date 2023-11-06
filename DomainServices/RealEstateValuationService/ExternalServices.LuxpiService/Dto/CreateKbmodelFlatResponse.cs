namespace DomainServices.RealEstateValuationService.ExternalServices.LuxpiService.Dto;

public sealed class CreateKbmodelFlatResponse
{
    public bool NoPriceAvailable { get; set; }
    public int ResultPrice { get; set; }
    public long ValuationId { get; set; }
}

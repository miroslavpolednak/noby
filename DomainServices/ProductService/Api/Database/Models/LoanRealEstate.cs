namespace DomainServices.ProductService.Api.Database.Models;

internal sealed class LoanRealEstate
{
    public long RealEstateId { get; set; }

    public long RealEstateTypeId { get; set; }

    public int RealEstatePurchaseTypeId { get; set; }
}
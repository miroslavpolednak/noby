namespace DomainServices.ProductService.Api.Database.Models;

internal sealed class Collateral
{
    public long ProductId { get; set; }

    public long? RealEstateTypeId { get; set; }
}
namespace DomainServices.ProductService.Api.Database.Models;

internal class LoanStatement
{
    public long Id { get; set; }
    public string? Street { get; set; }
    public string? StreetNumber { get; set; }
    public string? HouseNumber { get; set; }
    public string? Postcode { get; set; }
    public string? City { get; set; }
    public short? CountryId { get; set; }
    public string? AddressPointId { get; set; }
}
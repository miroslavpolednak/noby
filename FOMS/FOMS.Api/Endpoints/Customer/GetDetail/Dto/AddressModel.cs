namespace FOMS.Api.Endpoints.Customer.GetDetail.Dto;

public class AddressModel
{
    public int? AddressTypeId { get; set; }
    public bool IsPrimary { get; set; }
    public string? Street { get; set; }
    public string? BuildingIdentificationNumber { get; set; }
    public string? LandRegistryNumber { get; set; }
    public string? Postcode  { get; set; }
    public string? City { get; set; }
    public int? countryId  { get; set; }
}
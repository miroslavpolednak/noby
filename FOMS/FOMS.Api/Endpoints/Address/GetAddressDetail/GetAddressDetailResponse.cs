namespace FOMS.Api.Endpoints.Address.GetAddressDetail;

public sealed class GetAddressDetailResponse
{
    public int? AddressTypeId { get; set; }

    public string? Street { get; set; }

    public string? BuildingIdentificationNumber { get; set; }

    public string? LandRegistryNumber { get; set; }

    public string? Postcode { get; set; }

    public string? City { get; set; }

    public int? CountryId { get; set; }
}

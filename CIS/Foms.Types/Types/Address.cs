namespace CIS.Foms.Types;

public class Address
{
    public int? AddressTypeId { get; set; }

    public string? Street { get; set; }
    
    public string? BuildingIdentificationNumber { get; set; }
    
    public string? LandRegistryNumber { get; set; }
    
    public string? Postcode { get; set; }
    
    public string? City { get; set; }
    
    public int? CountryId { get; set; }

    public string? CityDistrict { get; set; }

    public string? PragueDistrict { get; set; }

    public string? CountrySubdivision { get; set; }

    public DateTime? PrimaryAddressFrom { get; set; }

    public string? AddressPointId { get; set; }
}

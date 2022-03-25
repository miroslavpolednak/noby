namespace CIS.Foms.Types;

public class Address
{
    public string? Street { get; set; }
    
    public int? BuildingIdentificationNumber { get; set; }
    
    public int? LandRegistryNumber { get; set; }
    
    public string? Postcode { get; set; }
    
    public string? City { get; set; }
    
    public int? CountryId { get; set; }
}

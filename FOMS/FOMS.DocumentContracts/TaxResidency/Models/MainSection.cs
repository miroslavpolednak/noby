namespace FOMS.DocumentContracts.TaxResidency.Models;

public sealed class MainSection
{
    public bool IsNotCzechTaxResident { get; set;}
    public bool IsUsPerson { get; set;}  
    public int? CountryOfBirth { get; set; }
    public SharedModels.Address? Address { get; set; }
}

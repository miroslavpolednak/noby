namespace FOMS.DocumentContracts.SharedModels;

public class PersonalData
{
    public bool IsNaturalPerson { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? BirthNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? PlaceOfBirth { get; set; }
    public string? DegreeBefore { get; set; }
    public string? DegreeAfter { get; set; }
}

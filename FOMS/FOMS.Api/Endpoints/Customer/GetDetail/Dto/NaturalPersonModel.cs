namespace FOMS.Api.Endpoints.Customer.GetDetail.Dto;

public class NaturalPersonModel
{
    public string? BirthNumber { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public int? DegreeBeforeId  { get; set; }
    public int? DegreeAfterId  { get; set; }
    public string? BirthName  { get; set; }
    public string? PlaceOfBirth { get; set; }
    public int? BirthCountryId  { get; set; }
    public CIS.Foms.Enums.Genders Gender { get; set; }
    public int? MaritalStatusStateId { get; set; }
    public List<int>? CitizenshipCountriesId { get; set; }
}
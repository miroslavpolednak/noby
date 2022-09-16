namespace FOMS.Api.Endpoints.SalesArrangement.Dto;

public class CustomerListItem
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public int CustomerRoleId { get; set; }
    public string? BirthNumber { get; set; }
    public string? PlaceOfBirth { get; set; }   
    public int? MaritalStatusId { get; set; }
    public CIS.Foms.Types.Address? MainAddress { get; set; }
    public CIS.Foms.Types.Address? ContactAddress { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public List<CIS.Foms.Types.CustomerIdentity>? Identities { get; set; }
}
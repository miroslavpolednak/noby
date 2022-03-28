namespace FOMS.Api.Endpoints.SalesArrangement.Dto;

public class CustomerListItem
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public List<CIS.Foms.Types.CustomerIdentity>? Identities { get; set; }
}
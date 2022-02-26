namespace FOMS.Api.Endpoints.Household.Dto;

public class Customer
{
    /// <summary>
    /// CustomerOnSAId
    /// </summary>
    public int? Id { get; set; }
    
    /// <summary>
    /// Identity klienta v KB nebo MP
    /// </summary>
    public List<CIS.Foms.Types.CustomerIdentity>? Identities { get; set; }
    
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    
}
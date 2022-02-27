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
    
    /// <summary>
    /// Jmeno klienta
    /// </summary>
    /// <example>John</example>
    public string? FirstName { get; set; }
    
    /// <summary>
    /// Prijmeni klienta
    /// </summary>
    /// <example>Doe</example>
    public string? LastName { get; set; }
    
    /// <summary>
    /// Datum narozeni klienta
    /// </summary>
    /// <example>2002-00-01T00:00:00.000Z</example>
    public DateTime? DateOfBirth { get; set; }
    
}
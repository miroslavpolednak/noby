namespace FOMS.Api.Endpoints.Household.Dto;

public class CustomerInHousehold
{
    /// <summary>
    /// CustomerOnSAId
    /// </summary>
    public int? CustomerOnSAId { get; set; }

    /// <summary>
    /// Role klienta
    /// </summary>
    /// <example>1</example>
    public int RoleId { get; set; }

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
    /// <example>2002-01-01T00:00:00.000Z</example>
    public DateTime? DateOfBirth { get; set; }
    
}
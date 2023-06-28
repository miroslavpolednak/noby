using System.ComponentModel.DataAnnotations;

namespace NOBY.Api.Endpoints.Customer.Identify;

public sealed class IdentifyRequest
    : IRequest<SearchCustomers.Dto.CustomerInList?>
{
    /// <summary>
    /// Jméno customera
    /// </summary>
    [Required]
    public string FirstName { get; set; } = "";

    /// <summary>
    /// Příjmení customera
    /// </summary>
    [Required]
    public string LastName { get; set; } = "";

    /// <summary>
    /// Datum narození FO
    /// </summary>
    [Required]
    public DateTime? DateOfBirth { get; set; }

    /// <summary>
    /// Země vydání dokladu
    /// </summary>
    [Required]
    public int IssuingCountryId { get; set; }

    /// <summary>
    /// Typ dokladu
    /// </summary>
    [Required]
    public int IdentificationDocumentTypeId { get; set; }

    /// <summary>
    /// Číslo dokladu
    /// </summary>
    [Required]
    public string IdentificationDocumentNumber { get; set; } = "";

    /// <summary>
    /// Rodné číslo
    /// </summary>
    public string? BirthNumber { get; set; }

    /// <summary>
    /// ID klienta v MP nebo KB
    /// </summary>
    public CIS.Foms.Types.CustomerIdentity? Identity { get; set; }
}

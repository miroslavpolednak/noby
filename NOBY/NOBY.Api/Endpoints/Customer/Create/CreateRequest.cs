using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NOBY.Api.Endpoints.Customer.Create;

public class CreateRequest
    : IRequest<CreateResponse>
{
    /// <summary>
    /// Příznak tvrdého vytvoření customera.
    /// </summary>
    /// <example>true</example>
    [DefaultValue(false)]
    public bool HardCreate { get; set; }

    /// <summary>
    /// Id customera na sales arrangementu
    /// </summary>
    /// <example>123456</example>
    [Required]
    public int CustomerOnSAId { get; set; }

    /// <summary>
    /// Datum narození
    /// </summary>
    /// <example>2000-02-01</example>
    [Required]
    public DateTime BirthDate { get; set; }

    /// <summary>
    /// Jméno
    /// </summary>
    /// <example>Jidáš</example>
    [Required]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Příjmení
    /// </summary>
    /// <example>Skočdopole</example>
    [Required]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Místo narození
    /// </summary>
    /// <example>Praha</example>
    public string? BirthPlace { get; set; }

    /// <summary>
    /// Pohlaví\n\n<small>Enum Values</small><ul><li>0 - Unknown</li><li>1 - Male</li><li>2 - Female</li></ul>
    /// </summary>
    [Required]
    public int GenderId { get; set; }

    /// <summary>
    /// Státní příslušnost/občanství
    /// </summary>
    /// <example>16</example>
    public int? CitizenshipCountryId { get; set; }

    public SharedDto.ContactsDto? Contacts { get; set; }

    /// <summary>
    /// Rodné číslo CZ
    /// </summary>
    public string? BirthNumber { get; set; }

    /// <summary>
    /// Trvalá adresa v ČR
    /// </summary>
    [Required]
    public CIS.Foms.Types.AddressRequired? PrimaryAddress { get; set; }

    [Required]
    public SharedDto.IdentificationDocumentFullRequired? IdentificationDocument { get; set; }
}

namespace FOMS.Api.Endpoints.Customer.Create;

public class CreateRequest
    : IRequest<CreateResponse>
{
    public bool HardCreate { get; set; }

    /// <summary>
    /// Id customera na sales arrangementu
    /// </summary>
    public int CustomerOnSAId { get; set; }

    /// <summary>
    /// Datum narození
    /// </summary>
    public DateTime? BirthDate { get; set; }

    /// <summary>
    /// Jméno
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Příjmení
    /// </summary>
    public string LastName { get; set; } = String.Empty;

    /// <summary>
    /// Místo narození
    /// </summary>
    public string? BirthPlace { get; set; }

    /// <summary>
    /// Pohlaví
    /// </summary>
    public int GenderId { get; set; }

    /// <summary>
    /// Státní příslušnost/občanství
    /// </summary>
    public int CitizenshipCountryId { get; set; }

    /// <summary>
    /// E-mail
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Mobil
    /// </summary>
    public string? Mobile { get; set; }

    /// <summary>
    /// Rodné číslo CZ
    /// </summary>
    public string? BirthNumber { get; set; }

    /// <summary>
    /// Trvalá adresa v ČR
    /// </summary>
    public CIS.Foms.Types.Address? PrimaryAddress { get; set; }

    public Dto.IdentificationDocument? IdentificationDocument { get; set; }
}

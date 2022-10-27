namespace FOMS.Api.Endpoints.Offer.LinkModelation;

public class LinkModelationRequest
    : IRequest
{
    public int SalesArrangementId { get; set; }

    public int OfferId { get; set; }

    /// <summary>
    /// Jméno
    /// </summary>
    /// <example>Jonatán</example>
    public string? FirstName { get; set; }

    /// <summary>
    /// Příjmení
    /// </summary>
    /// <example>Skočdopole</example>
    public string? LastName { get; set; }

    /// <summary>
    /// Datum narození
    /// </summary>
    /// <example>2002-10-26</example>
    public DateTime? DateOfBirth { get; set; }

    /// <example>777111222</example>
    public string? PhoneNumberForOffer { get; set; }

    /// <example>jonatan.skocdopole@mpss.cz</example>
    public string? EmailForOffer { get; set; }
}

namespace NOBY.Api.Endpoints.Offer.LinkModelation;

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

    public SharedDto.ContactsDto? OfferContacts { get; set; }
}

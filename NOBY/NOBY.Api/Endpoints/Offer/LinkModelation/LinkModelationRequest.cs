using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.Offer.LinkModelation;

public class LinkModelationRequest
    : IRequest
{
    [JsonIgnore]
    internal int SalesArrangementId { get; set; }

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

    public NOBY.Dto.ContactsDto? OfferContacts { get; set; }

    /// <summary>
    /// Komentář k Cenové výjimce
    /// </summary>
    /// <example>Prosím o slevu, jde o dlouhodobého loajálního klienta</example>
    public string? IndividualPriceCommentLastVersion { get; set; }

    internal LinkModelationRequest InfuseId(int salesArrangementId)
    {
        this.SalesArrangementId = salesArrangementId;
        return this;
    }
}

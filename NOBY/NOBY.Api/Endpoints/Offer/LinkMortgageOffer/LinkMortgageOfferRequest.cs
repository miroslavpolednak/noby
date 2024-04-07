using NOBY.Api.Endpoints.Offer.SharedDto.LinkModelation;

namespace NOBY.Api.Endpoints.Offer.LinkMortgageOffer;

public class LinkMortgageOfferRequest : LinkMortgageBaseRequest<LinkMortgageOfferRequest>
{
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

    public Dto.ContactsDto? OfferContacts { get; set; }

    protected override LinkMortgageOfferRequest GetRequestInstance() => this;
}
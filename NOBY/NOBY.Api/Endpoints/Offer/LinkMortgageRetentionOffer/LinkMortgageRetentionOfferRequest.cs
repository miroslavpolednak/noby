using NOBY.Api.Endpoints.Offer.SharedDto.LinkModelation;

namespace NOBY.Api.Endpoints.Offer.LinkMortgageRetentionOffer;

public class LinkMortgageRetentionOfferRequest : LinkMortgageBaseRequest<LinkMortgageRetentionOfferRequest>
{
    /// <summary>
    /// Komentář k Cenové výjimce
    /// </summary>
    /// <example>Prosím o slevu, jde o dlouhodobého loajálního klienta</example>
    public string? IndividualPriceCommentLastVersion { get; set; }

    protected override LinkMortgageRetentionOfferRequest GetRequestInstance() => this;
}
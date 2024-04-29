using NOBY.Api.Endpoints.Offer.SharedDto.LinkModelation;

namespace NOBY.Api.Endpoints.Offer.LinkMortgageExtraPayment;

public class LinkMortgageExtraPaymentRequest : LinkMortgageBaseRequest<LinkMortgageExtraPaymentRequest>
{
    /// <summary>
    /// Komentář k Cenové výjimce
    /// </summary>
    /// <example>Prosím o slevu, jde o dlouhodobého loajálního klienta</example>
    public string? IndividualPriceCommentLastVersion { get; set; }

    protected override LinkMortgageExtraPaymentRequest GetRequestInstance() => this;
}
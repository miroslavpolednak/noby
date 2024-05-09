using NOBY.Api.Endpoints.Offer.SharedDto.LinkModelation;
using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.Offer.LinkMortgageOffer;

public class LinkMortgageOfferRequest : LinkMortgageBaseRequest<LinkMortgageOfferRequest>
{
    [JsonIgnore]
    internal int SalesArrangementId { get; set; }

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

	internal LinkMortgageOfferRequest InfuseId(long caseId, int salesArrangementId)
	{
		CaseId = caseId;
        SalesArrangementId = salesArrangementId;

		return this;
	}

	protected override LinkMortgageOfferRequest GetRequestInstance() => this;
}
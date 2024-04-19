using DomainServices.OfferService.Contracts;
using SharedTypes.Enums;

namespace NOBY.Dto.Refinancing;

public sealed class RefinancingOfferDetail
{
    public int OfferId { get; set; }

    /// <summary>
    /// Platnost nabídky do
    /// </summary>
    public DateTime? ValidTo { get; set; }

    /// <summary>
    /// Jedná se o zákonné sdělení
    /// </summary>
    public bool IsLegalNotice { get; set; }

    /// <summary>
    /// Jedná se o Aktuální nabídku
    /// </summary>
    public bool IsCurrent { get; set; }

    /// <summary>
    /// Jedná se o sdělenou nabídku
    /// </summary>
    public bool IsCommunicated { get; set; }

    /// <summary>
    /// Jedná se o likovanou nabídku
    /// </summary>
    public bool IsLiked { get; set; }

    /// <summary>
    /// Jedná se o zvolenou nabídku
    /// </summary>
    public bool IsSelected { get; set; }

    /// <summary>
    /// Jedná se o výchozí nabídku
    /// </summary>
    public bool IsLegalNoticeDefault { get; set; }

    public int FixedRatePeriod { get; set; }

    public decimal InterestRate { get; set; }

    /// <summary>
    /// Sleva ze sazby
    /// </summary>
    /// <example>0.2</example>
    public decimal? InterestRateDiscount { get; set; }

    /// <summary>
    /// Sleva ze sazby
    /// </summary>
    /// <example>4.5</example>
    public decimal? InterestRateDiscounted { get => InterestRateDiscount.HasValue ? InterestRate - InterestRateDiscount : null; }

    /// <summary>
    /// Výše měsíční splátky.
    /// </summary>
    public decimal LoanPaymentAmount { get; set; }

    /// <summary>
    /// Výše měsíční splátky se zohledněním IC.
    /// </summary>
    public decimal? LoanPaymentAmountDiscounted { get; set; }

    public static RefinancingOfferDetail CreateRefixationOffer(GetOfferListResponse.Types.GetOfferListItem offer)
    {
        var result =  new RefinancingOfferDetail
        {
            OfferId = offer.Data.OfferId,
            ValidTo = offer.Data.ValidTo,
            FixedRatePeriod = offer.MortgageRefixation.SimulationInputs.FixedRatePeriod,
            InterestRate = offer.MortgageRefixation.SimulationInputs.InterestRate,
            InterestRateDiscount = offer.MortgageRefixation.SimulationInputs.InterestRateDiscount,
            LoanPaymentAmount = offer.MortgageRefixation.SimulationResults.LoanPaymentAmount,
            LoanPaymentAmountDiscounted = offer.MortgageRefixation.SimulationResults.LoanPaymentAmountDiscounted
        };
        SetFlags(result, offer.Data.Flags);
        return result;
    }

    public static void SetFlags(RefinancingOfferDetail instance, int offerFlags)
    {
        var flags = (OfferFlagTypes)offerFlags;

        instance.IsCommunicated = flags.HasFlag(OfferFlagTypes.Communicated);
        instance.IsCurrent = flags.HasFlag(OfferFlagTypes.Current);
        instance.IsLegalNotice = flags.HasFlag(OfferFlagTypes.LegalNotice);
        instance.IsLiked = flags.HasFlag(OfferFlagTypes.Liked);
        instance.IsSelected = flags.HasFlag(OfferFlagTypes.Selected);
        instance.IsLegalNoticeDefault = flags.HasFlag(OfferFlagTypes.LegalNoticeDefault);
    }
}

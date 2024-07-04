using SharedTypes.Enums;

namespace NOBY.ApiContracts;

public partial class RefinancingSharedOfferDetail
{
    public static RefinancingSharedOfferDetail CreateRefixationOffer(DomainServices.OfferService.Contracts.GetOfferListResponse.Types.GetOfferListItem offer)
    {
        var result = new RefinancingSharedOfferDetail
        {
            OfferId = offer.Data.OfferId,
            ValidTo = offer.Data.ValidTo,
            FixedRatePeriod = offer.MortgageRefixation.SimulationInputs.FixedRatePeriod,
            InterestRate = offer.MortgageRefixation.SimulationInputs.InterestRate,
            InterestRateDiscount = offer.MortgageRefixation.SimulationInputs.InterestRateDiscount,
            LoanPaymentAmount = offer.MortgageRefixation.SimulationResults.LoanPaymentAmount,
            LoanPaymentAmountDiscounted = offer.MortgageRefixation.SimulationResults.LoanPaymentAmountDiscounted != offer.MortgageRefixation.SimulationResults.LoanPaymentAmount ? offer.MortgageRefixation.SimulationResults.LoanPaymentAmountDiscounted : null
        };
        SetFlags(result, offer.Data.Flags);
        return result;
    }

    public static void SetFlags(RefinancingSharedOfferDetail instance, int offerFlags)
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

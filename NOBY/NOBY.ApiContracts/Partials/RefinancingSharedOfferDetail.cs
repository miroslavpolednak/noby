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
        var flags = (EnumOfferFlagTypes)offerFlags;

        instance.IsCommunicated = flags.HasFlag(EnumOfferFlagTypes.Communicated);
        instance.IsCurrent = flags.HasFlag(EnumOfferFlagTypes.Current);
        instance.IsLegalNotice = flags.HasFlag(EnumOfferFlagTypes.LegalNotice);
        instance.IsLiked = flags.HasFlag(EnumOfferFlagTypes.Liked);
        instance.IsSelected = flags.HasFlag(EnumOfferFlagTypes.Selected);
        instance.IsLegalNoticeDefault = flags.HasFlag(EnumOfferFlagTypes.LegalNoticeDefault);
    }
}

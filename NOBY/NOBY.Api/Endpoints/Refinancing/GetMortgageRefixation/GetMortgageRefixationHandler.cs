using DomainServices.OfferService.Clients.v1;
using DomainServices.OfferService.Contracts;
using NOBY.Services.MortgageRefinancing;

namespace NOBY.Api.Endpoints.Refinancing.GetMortgageRefixation;

internal sealed class GetMortgageRefixationHandler(
    TimeProvider _timeProvider,
    IOfferServiceClient _offerService,
    MortgageRefinancingDataService _refinancingDataService,
    Services.ResponseCodes.ResponseCodesService _responseCodes)
        : IRequestHandler<GetMortgageRefixationRequest, RefinancingGetMortgageRefixationResponse>
{
    public async Task<RefinancingGetMortgageRefixationResponse> Handle(GetMortgageRefixationRequest request, CancellationToken cancellationToken)
    {
        // sesbirat vsechna potrebna data
        var data = await _refinancingDataService.GetRefinancingData(request.CaseId, request.ProcessId, EnumRefinancingTypes.MortgageRefixation, cancellationToken);

        // vytvorit a naplnit zaklad response modelu
        var response = data.UpdateBaseResponseModel(new RefinancingGetMortgageRefixationResponse());

        // refixation specific data
        response.ResponseCodes = await _responseCodes.GetMortgageResponseCodes(request.CaseId, OfferTypes.MortgageRefixation, cancellationToken);
        response.Document = await _refinancingDataService.CreateSigningDocument(data, EnumRefinancingTypes.MortgageRefixation, data.Process?.MortgageRefixation?.DocumentEACode, data.Process?.MortgageRefixation?.DocumentId);
        response.IndividualPriceCommentLastVersion = data.SalesArrangement?.Refixation?.IndividualPriceCommentLastVersion;
        response.Comment = data.SalesArrangement?.Refixation?.Comment;

        if (((decimal?)data.ActivePriceException?.LoanInterestRate?.LoanInterestRateDiscount ?? 0) > 0)
        {
            response.InterestRateDiscount = data.ActivePriceException?.LoanInterestRate?.LoanInterestRateDiscount;
        }

        // zjistit rate ICcka
        decimal? icRate = null;
        if (data.ActivePriceException is not null)
        {
            icRate = response.InterestRateDiscount;
        }

        var offers = await getOffers(request.CaseId, cancellationToken);

        // seznam nabidek
        response.Offers = offers
            ?.Select(createRefixationOffer)
            .OrderBy(t => t.FixedRatePeriod)
            .ToList();

        response.Document.IsGenerateDocumentEnabled = response.Document.IsGenerateDocumentEnabled && (offers?.Any(t => ((EnumOfferFlagTypes)t.Data.Flags).HasFlag(EnumOfferFlagTypes.Selected)) ?? false);
        response.CommunicatedOffersValidTo = offers?.FirstOrDefault(t => ((EnumOfferFlagTypes)t.Data.Flags).HasFlag(EnumOfferFlagTypes.Communicated))?.Data.ValidTo;
        response.LegalNoticeGeneratedDate = offers?.FirstOrDefault(t => ((EnumOfferFlagTypes)t.Data.Flags).HasFlag(EnumOfferFlagTypes.LegalNotice))?.MortgageRefixation?.BasicParameters?.LegalNoticeGeneratedDate;
        response.ContainsInconsistentIndividualPriceData = !(response.Offers?.All(t => t.IsLegalNotice || t.InterestRateDiscount == icRate) ?? true);

        return response;
    }

    private async Task<List<GetOfferListResponse.Types.GetOfferListItem>> getOffers(long caseId, CancellationToken cancellationToken)
    {
        return (await _offerService.GetOfferList(caseId, OfferTypes.MortgageRefixation, false, cancellationToken: cancellationToken))
            .Where(t => !(t.Data.ValidTo < _timeProvider.GetLocalNow().Date))
            .ToList();
    }

    private static RefinancingGetMortgageRefixationOfferDetail createRefixationOffer(GetOfferListResponse.Types.GetOfferListItem offer)
    {
        var result = new RefinancingGetMortgageRefixationOfferDetail
        {
            OfferId = offer.Data.OfferId,
            ValidTo = offer.Data.ValidTo,
            FixedRatePeriod = offer.MortgageRefixation.SimulationInputs.FixedRatePeriod,
            InterestRate = offer.MortgageRefixation.SimulationInputs.InterestRate,
            InterestRateDiscount = offer.MortgageRefixation.SimulationInputs.InterestRateDiscount,
            InterestRateDiscounted = offer.MortgageRefixation.SimulationInputs.InterestRate != offer.MortgageRefixation.SimulationInputs.InterestRateDiscount && ((decimal?)offer.MortgageRefixation.SimulationInputs.InterestRateDiscount).HasValue ? offer.MortgageRefixation.SimulationInputs.InterestRate - offer.MortgageRefixation.SimulationInputs.InterestRateDiscount : null,
            LoanPaymentAmount = offer.MortgageRefixation.SimulationResults.LoanPaymentAmount,
            LoanPaymentAmountDiscounted = offer.MortgageRefixation.SimulationResults.LoanPaymentAmountDiscounted != offer.MortgageRefixation.SimulationResults.LoanPaymentAmount ? offer.MortgageRefixation.SimulationResults.LoanPaymentAmountDiscounted : null
        };

        setFlags(result, offer.Data.Flags);

        return result;
    }

    private static void setFlags(RefinancingGetMortgageRefixationOfferDetail instance, int offerFlags)
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

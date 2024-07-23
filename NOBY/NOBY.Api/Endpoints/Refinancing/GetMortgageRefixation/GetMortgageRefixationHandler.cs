using DomainServices.OfferService.Clients.v1;
using DomainServices.OfferService.Contracts;
using NOBY.Services.MortgageRefinancing;

namespace NOBY.Api.Endpoints.Refinancing.GetMortgageRefixation;

internal sealed class GetMortgageRefixationHandler(
    TimeProvider _timeProvider,
    IOfferServiceClient _offerService,
    MortgageRefinancingDataService _refinancingDataService,
    Services.ResponseCodes.ResponseCodesService _responseCodes)
        : IRequestHandler<GetMortgageRefixationRequest, GetMortgageRefixationResponse>
{
    public async Task<GetMortgageRefixationResponse> Handle(GetMortgageRefixationRequest request, CancellationToken cancellationToken)
    {
        // sesbirat vsechna potrebna data
        var data = await _refinancingDataService.GetRefinancingData(request.CaseId, request.ProcessId, RefinancingTypes.MortgageRefixation, cancellationToken);

        // vytvorit a naplnit zaklad response modelu
        var response = data.UpdateBaseResponseModel(new GetMortgageRefixationResponse());

        // refixation specific data
        response.ResponseCodes = await _responseCodes.GetMortgageResponseCodes(request.CaseId, OfferTypes.MortgageRefixation, cancellationToken);
        response.Document = await _refinancingDataService.CreateSigningDocument(data, RefinancingTypes.MortgageRefixation, data.Process?.MortgageRefixation?.DocumentEACode, data.Process?.MortgageRefixation?.DocumentId);
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
            ?.Select(Dto.Refinancing.RefinancingOfferDetail.CreateRefixationOffer)
            .OrderBy(t => t.FixedRatePeriod)
            .ToList();

        response.Document.IsGenerateDocumentEnabled = response.Document.IsGenerateDocumentEnabled && (offers?.Any(t => ((OfferFlagTypes)t.Data.Flags).HasFlag(OfferFlagTypes.Selected)) ?? false);
        response.CommunicatedOffersValidTo = offers?.FirstOrDefault(t => ((OfferFlagTypes)t.Data.Flags).HasFlag(OfferFlagTypes.Communicated))?.Data.ValidTo;
        response.LegalNoticeGeneratedDate = offers?.FirstOrDefault(t => ((OfferFlagTypes)t.Data.Flags).HasFlag(OfferFlagTypes.LegalNotice))?.MortgageRefixation?.BasicParameters?.LegalNoticeGeneratedDate;
        response.ContainsInconsistentIndividualPriceData = !(response.Offers?.All(t => t.IsLegalNotice || t.InterestRateDiscount == icRate) ?? true);

        return response;
    }

    private async Task<List<GetOfferListResponse.Types.GetOfferListItem>> getOffers(long caseId, CancellationToken cancellationToken)
    {
        return (await _offerService.GetOfferList(caseId, OfferTypes.MortgageRefixation, false, cancellationToken))
            .Where(t => !(t.Data.ValidTo < _timeProvider.GetLocalNow().Date))
            .ToList();
    }
}

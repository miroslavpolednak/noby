using DomainServices.CaseService.Clients.v1;
using DomainServices.OfferService.Clients.v1;
using NOBY.Services.MortgageRefinancing;

namespace NOBY.Api.Endpoints.Refinancing.GetMortgageRefixation;

internal sealed class GetMortgageRefixationHandler(
    TimeProvider _timeProvider,
    IOfferServiceClient _offerService,
    ICaseServiceClient _caseService,
    MortgageRefinancingWorkflowService _refinancingWorkflowService,
    Services.ResponseCodes.ResponseCodesService _responseCodes)
        : IRequestHandler<GetMortgageRefixationRequest, GetMortgageRefixationResponse>
{
    public async Task<GetMortgageRefixationResponse> Handle(GetMortgageRefixationRequest request, CancellationToken cancellationToken)
    {
        var retentionData = await _refinancingWorkflowService.GetRefinancingData(request.CaseId, request.ProcessId, RefinancingTypes.MortgageRefixation, cancellationToken);

        GetMortgageRefixationResponse response = new()
        {
            SalesArrangementId = retentionData.SalesArrangement?.SalesArrangementId,
            ResponseCodes = await _responseCodes.GetMortgageResponseCodes(request.CaseId, DomainServices.OfferService.Contracts.OfferTypes.MortgageRefixation, cancellationToken),
            IsReadOnly = retentionData.RefinancingState == RefinancingStates.RozpracovanoVNoby,
            Tasks = retentionData.Tasks,
            IndividualPriceCommentLastVersion = retentionData.SalesArrangement?.Refixation?.IndividualPriceCommentLastVersion,
            Comment = retentionData.SalesArrangement?.Refixation?.Comment
        };

        // zjistit rate ICcka
        decimal? icRate = null;
        if (retentionData.ActivePriceExceptionTaskIdSb.HasValue)
        {
            var taskDetail = await _caseService.GetTaskDetail(retentionData.ActivePriceExceptionTaskIdSb.Value, cancellationToken);
            icRate = taskDetail.TaskDetail.PriceException?.LoanInterestRate?.LoanInterestRateDiscount;
        }

        var offers = (await _offerService.GetOfferList(request.CaseId, DomainServices.OfferService.Contracts.OfferTypes.MortgageRefixation, false, cancellationToken))
            .Where(t => t.Data.ValidTo >= _timeProvider.GetLocalNow().Date)
            .ToList();

        // seznam nabidek
        response.Offers = offers?.Select(Dto.Refinancing.RefinancingOfferDetail.CreateRefixationOffer).ToList();
        response.CommunicatedOffersValidTo = offers?.FirstOrDefault(t => ((OfferFlagTypes)t.Data.Flags).HasFlag(OfferFlagTypes.Communicated))?.Data.ValidTo;
        response.LegalNoticeGeneratedDate = offers?.FirstOrDefault(t => ((OfferFlagTypes)t.Data.Flags).HasFlag(OfferFlagTypes.Communicated))?.MortgageRefixation?.BasicParameters?.LegalNoticeGeneratedDate;
        response.ContainsInconsistentIndividualPriceData = !(response.Offers?.All(t => t.InterestRateDiscount == icRate) ?? true);

        return response;
    }
}

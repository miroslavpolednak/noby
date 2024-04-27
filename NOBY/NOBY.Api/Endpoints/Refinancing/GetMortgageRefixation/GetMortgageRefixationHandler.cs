﻿using DomainServices.OfferService.Clients.v1;
using NOBY.Services.MortgageRefinancing;

namespace NOBY.Api.Endpoints.Refinancing.GetMortgageRefixation;

internal sealed class GetMortgageRefixationHandler(
    TimeProvider _timeProvider,
    IOfferServiceClient _offerService,
    MortgageRefinancingWorkflowService _refinancingWorkflowService,
    Services.ResponseCodes.ResponseCodesService _responseCodes)
        : IRequestHandler<GetMortgageRefixationRequest, GetMortgageRefixationResponse>
{
    public async Task<GetMortgageRefixationResponse> Handle(GetMortgageRefixationRequest request, CancellationToken cancellationToken)
    {
        var retentionData = await _refinancingWorkflowService.GetRefinancingData(request.CaseId, request.ProcessId, RefinancingTypes.MortgageRefixation, cancellationToken);

        GetMortgageRefixationResponse response = new()
        {
            RefinancingStateId = (int)retentionData.RefinancingState,
            SalesArrangementId = retentionData.SalesArrangement?.SalesArrangementId,
            ResponseCodes = await _responseCodes.GetMortgageResponseCodes(request.CaseId, DomainServices.OfferService.Contracts.OfferTypes.MortgageRefixation, cancellationToken),
            IsReadOnly = retentionData.RefinancingState != RefinancingStates.RozpracovanoVNoby,
            Tasks = retentionData.Tasks,
            IndividualPriceCommentLastVersion = retentionData.SalesArrangement?.Refixation?.IndividualPriceCommentLastVersion,
            Comment = retentionData.SalesArrangement?.Refixation?.Comment,
            InterestRateDiscount = retentionData.ActivePriceException?.LoanInterestRate?.LoanInterestRateDiscount,
            IsPriceExceptionActive = retentionData.ActivePriceException is not null
        };

        // zjistit rate ICcka
        decimal? icRate = null;
        if (retentionData.ActivePriceException is not null)
        {
            icRate = retentionData.ActivePriceException.LoanInterestRate?.LoanInterestRateDiscount;
        }

        var offers = (await _offerService.GetOfferList(request.CaseId, DomainServices.OfferService.Contracts.OfferTypes.MortgageRefixation, false, cancellationToken))
            .Where(t => !(t.Data.ValidTo < _timeProvider.GetLocalNow().Date))
            .ToList();

        // seznam nabidek
        response.Offers = offers?.Select(Dto.Refinancing.RefinancingOfferDetail.CreateRefixationOffer).ToList();
        response.CommunicatedOffersValidTo = offers?.FirstOrDefault(t => ((OfferFlagTypes)t.Data.Flags).HasFlag(OfferFlagTypes.Communicated))?.Data.ValidTo;
        response.LegalNoticeGeneratedDate = offers?.FirstOrDefault(t => ((OfferFlagTypes)t.Data.Flags).HasFlag(OfferFlagTypes.Communicated))?.MortgageRefixation?.BasicParameters?.LegalNoticeGeneratedDate;
        response.ContainsInconsistentIndividualPriceData = !(response.Offers?.All(t => t.IsLegalNotice || t.InterestRateDiscount == icRate) ?? true);

        return response;
    }
}

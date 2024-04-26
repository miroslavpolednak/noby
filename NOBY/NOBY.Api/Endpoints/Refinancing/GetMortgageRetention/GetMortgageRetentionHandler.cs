using DomainServices.OfferService.Clients.v1;
using DomainServices.OfferService.Contracts;
using NOBY.Services.MortgageRefinancing;

namespace NOBY.Api.Endpoints.Refinancing.GetMortgageRetention;

internal sealed class GetMortgageRetentionHandler(
    IOfferServiceClient _offerService,
    Services.ResponseCodes.ResponseCodesService _responseCodes,
    MortgageRefinancingWorkflowService _refinancingWorkflowService)
        : IRequestHandler<GetMortgageRetentionRequest, GetMortgageRetentionResponse>
{
    public async Task<GetMortgageRetentionResponse> Handle(GetMortgageRetentionRequest request, CancellationToken cancellationToken)
    {
        var retentionData = await _refinancingWorkflowService.GetRefinancingData(request.CaseId, request.ProcessId, RefinancingTypes.MortgageRetention, cancellationToken);

        var response = new GetMortgageRetentionResponse
        {
            RefinancingStateId = (int)retentionData.RefinancingState,
            SalesArrangementId = retentionData.SalesArrangement?.SalesArrangementId,
            IsReadOnly = retentionData.RefinancingState != RefinancingStates.RozpracovanoVNoby,
            Tasks = retentionData.Tasks,
            ResponseCodes = await _responseCodes.GetMortgageResponseCodes(request.CaseId, OfferTypes.MortgageRefixation, cancellationToken),
            IndividualPriceCommentLastVersion = retentionData.SalesArrangement?.Retention?.IndividualPriceCommentLastVersion,
            Comment = retentionData.SalesArrangement?.Retention?.Comment,
            SignatureTypeDetailId = retentionData.SalesArrangement?.Retention?.SignatureTypeDetailId,
            // doplnit data simulace z procesu (pozdeji mozna prepsat offerou)
            InterestRate = (decimal?)retentionData.Process!.MortgageRetention.LoanInterestRate ?? 0M,
            LoanPaymentAmount = (decimal?)retentionData.Process.MortgageRetention.LoanPaymentAmount ?? 0M,
            LoanPaymentAmountDiscounted = retentionData.Process.MortgageRetention.LoanPaymentAmountFinal,
            FeeAmount = (decimal?)retentionData.Process.MortgageRetention.FeeSum ?? 0M,
            IsPriceExceptionActive = retentionData.ActivePriceException is not null
        };

        if (!string.IsNullOrEmpty(retentionData.Process.MortgageRetention?.DocumentId))
        {
            response.Document = new()
            {
                DocumentId = retentionData.Process.MortgageRetention.DocumentId,
                DocumentEACode = retentionData.Process.MortgageRetention.DocumentEACode ?? 0
            };
        }

        // aktivni IC
        if (retentionData.ActivePriceException is not null)
        {
            // rate
            response.InterestRateDiscount = retentionData.ActivePriceException.LoanInterestRate?.LoanInterestRateDiscount;

            // poplatek
            if ((retentionData.ActivePriceException.Fees?.Count ?? 0) != 0)
            {
                response.FeeAmountDiscounted = retentionData.ActivePriceException.Fees![0].FinalSum;
            }
        }

        // rozhodnuti pro generovani doc.
        response.IsGenerateDocumentEnabled = retentionData.SalesArrangement?.OfferId is not null &&
                                             retentionData.RefinancingState == RefinancingStates.RozpracovanoVNoby &&
                                             !response.IsPriceExceptionActive;

        // pokud existuje Offer
        if (retentionData.SalesArrangement?.OfferId is not null)
        {
            // detail offer
            var offerInstance = await _offerService.GetOffer(retentionData.SalesArrangement.OfferId.Value, cancellationToken);

            // nacpat data z offer do response
            response.InterestRateValidFrom = offerInstance.MortgageRetention.SimulationInputs.InterestRateValidFrom;

            response.ContainsInconsistentIndividualPriceData = 
                offerInstance.MortgageRetention.SimulationInputs.InterestRateDiscount != response.InterestRateDiscount
                || offerInstance.MortgageRetention.SimulationInputs.InterestRate != response.InterestRate
                || offerInstance.MortgageRetention.SimulationResults.LoanPaymentAmount != response.LoanPaymentAmount
                || offerInstance.MortgageRetention.BasicParameters.FeeAmount != response.FeeAmount
                || offerInstance.MortgageRetention.BasicParameters.FeeAmountDiscounted != response.FeeAmountDiscounted;
        }

        return response;
    }
}

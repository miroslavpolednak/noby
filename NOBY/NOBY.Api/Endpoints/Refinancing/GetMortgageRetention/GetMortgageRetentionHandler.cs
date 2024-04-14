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
            SalesArrangementId = retentionData.SalesArrangement?.SalesArrangementId,
            IsReadOnly = retentionData.RefinancingState == RefinancingStates.RozpracovanoVNoby,
            Tasks = retentionData.Tasks,
            ResponseCodes = await _responseCodes.GetMortgageResponseCodes(request.CaseId, OfferTypes.MortgageRefixation, cancellationToken),
            IndividualPriceCommentLastVersion = retentionData.SalesArrangement?.Retention?.IndividualPriceCommentLastVersion,
            Comment = retentionData.SalesArrangement?.Retention?.Comment,
            SignatureTypeDetailId = retentionData.SalesArrangement?.Retention?.SignatureTypeDetailId,
            DocumentId = retentionData.Process!.RefinancingProcess.RefinancingDocumentId,
            RefinancingDocumentEACode = retentionData.Process.RefinancingProcess.RefinancingDocumentEACode,
            // doplnit data simulace z procesu (pozdeji mozna prepsat offerou)
            InterestRate = (decimal?)retentionData.Process.RefinancingProcess.LoanInterestRate ?? 0M,
            InterestRateDiscount = (decimal?)retentionData.Process.RefinancingProcess.LoanInterestRateProvided,
            LoanPaymentAmount = (decimal?)retentionData.Process.RefinancingProcess.LoanPaymentAmount ?? 0M,
            LoanPaymentAmountDiscounted = retentionData.Process.RefinancingProcess.LoanPaymentAmountFinal,
            FeeAmount = (decimal?)retentionData.Process.RefinancingProcess.FeeSum ?? 0M,
            FeeAmountDiscounted = retentionData.Process.RefinancingProcess.FeeFinalSum,
            IsGenerateDocumentEnabled = retentionData.SalesArrangement?.OfferId is not null && retentionData.RefinancingState == RefinancingStates.RozpracovanoVNoby && retentionData.ActivePriceExceptionTaskIdSb.HasValue
        };

        // pokud existuje Offer
        if (retentionData.SalesArrangement?.OfferId is not null)
        {
            // detail offer
            var offerInstance = await _offerService.GetOffer(retentionData.SalesArrangement.OfferId.Value, cancellationToken);
            // nacpat data z offer do response misto puvodnich dat z procesu
            replaceTaskDataWithOfferData(response, offerInstance);

            if (retentionData.ActivePriceExceptionTaskIdSb.HasValue)
            {
                response.ContainsInconsistentIndividualPriceData = offerInstance.MortgageRetention.SimulationInputs.InterestRateDiscount != response.InterestRateDiscount || offerInstance.MortgageRetention.BasicParameters.FeeAmountDiscounted != response.FeeAmountDiscounted;

                response.InterestRateDiscount = offerInstance.MortgageRetention.SimulationInputs.InterestRateDiscount;
                response.LoanPaymentAmountDiscounted = offerInstance.MortgageRetention.SimulationResults.LoanPaymentAmountDiscounted;
                response.FeeAmountDiscounted = offerInstance.MortgageRetention.BasicParameters.FeeAmountDiscounted;
            }
        }

        return response;
    }

    /// <summary>
    /// Vytvoreni detail Offer
    /// </summary>
    private static void replaceTaskDataWithOfferData(GetMortgageRetentionResponse response, GetOfferResponse offerInstance)
    {
        response.FeeAmount = offerInstance.MortgageRetention.BasicParameters.FeeAmount;
        response.InterestRateValidFrom = offerInstance.MortgageRetention.SimulationInputs.InterestRateValidFrom;
        response.InterestRate = offerInstance.MortgageRetention.SimulationInputs.InterestRate;
        response.LoanPaymentAmount = offerInstance.MortgageRetention.SimulationResults.LoanPaymentAmount;
    }
}

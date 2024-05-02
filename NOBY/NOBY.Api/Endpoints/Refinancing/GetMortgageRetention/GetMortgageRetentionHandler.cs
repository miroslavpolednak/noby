using DomainServices.OfferService.Clients.v1;
using DomainServices.OfferService.Contracts;
using NOBY.Services.MortgageRefinancing;

namespace NOBY.Api.Endpoints.Refinancing.GetMortgageRetention;

internal sealed class GetMortgageRetentionHandler(
    IOfferServiceClient _offerService,
    Services.ResponseCodes.ResponseCodesService _responseCodes,
    MortgageRefinancingDataService _refinancingDataService)
        : IRequestHandler<GetMortgageRetentionRequest, GetMortgageRetentionResponse>
{
    public async Task<GetMortgageRetentionResponse> Handle(GetMortgageRetentionRequest request, CancellationToken cancellationToken)
    {
        // sesbirat vsechna potrebna data
        var data = await _refinancingDataService.GetRefinancingData(request.CaseId, request.ProcessId, RefinancingTypes.MortgageRetention, cancellationToken);

        // vytvorit a naplnit zaklad response modelu
        var response = data.UpdateBaseResponseModel(new GetMortgageRetentionResponse());

        // retention specific data
        response.ResponseCodes = await _responseCodes.GetMortgageResponseCodes(request.CaseId, OfferTypes.MortgageRefixation, cancellationToken);
        response.Document = await _refinancingDataService.CreateSigningDocument(data, RefinancingTypes.MortgageExtraPayment, data.Process?.MortgageRetention?.DocumentEACode, data.Process?.MortgageRetention?.DocumentId);
        response.IndividualPriceCommentLastVersion = data.SalesArrangement?.Retention?.IndividualPriceCommentLastVersion;
        response.Comment = data.SalesArrangement?.Retention?.Comment;
        response.InterestRate = (decimal?)data.Process!.MortgageRetention.LoanInterestRate ?? 0M;
        response.LoanPaymentAmount = (decimal?)data.Process.MortgageRetention.LoanPaymentAmount ?? 0M;
        response.FeeAmount = (decimal?)data.Process.MortgageRetention.FeeSum ?? 0M;
        
        if (((decimal?)data.Process.MortgageRetention.LoanPaymentAmountFinal ?? 0) > 0)
        {
            response.LoanPaymentAmountDiscounted = data.Process.MortgageRetention.LoanPaymentAmountFinal;
        }

        // IC rate
        if (((decimal?)data.ActivePriceException?.LoanInterestRate?.LoanInterestRateDiscount ?? 0) > 0)
        {
            response.InterestRateDiscount = data.ActivePriceException!.LoanInterestRate?.LoanInterestRateDiscount;
        }

        // IC poplatek
        if ((data.ActivePriceException?.Fees?.Count ?? 0) != 0)
        {
            response.FeeAmountDiscounted = data.ActivePriceException!.Fees![0].FinalSum;
        }

        // pokud existuje Offer
        if (data.SalesArrangement?.OfferId is not null)
        {
            // detail offer
            var offerInstance = await _offerService.GetOffer(data.SalesArrangement.OfferId.Value, cancellationToken);

            // nacpat data z offer do response
            response.InterestRateValidFrom = offerInstance.MortgageRetention.SimulationInputs.InterestRateValidFrom;

            // priznak zda se lysi offer od dat z procesu
            response.ContainsInconsistentIndividualPriceData = getContainsInconsistentIndividualPriceData(offerInstance, response);
        }

        return response;
    }

    private static bool getContainsInconsistentIndividualPriceData(GetOfferResponse offerInstance, GetMortgageRetentionResponse response)
    {
        return offerInstance.MortgageRetention.SimulationInputs.InterestRateDiscount != response.InterestRateDiscount
                || offerInstance.MortgageRetention.SimulationInputs.InterestRate != response.InterestRate
                || offerInstance.MortgageRetention.SimulationResults.LoanPaymentAmount != response.LoanPaymentAmount
                || offerInstance.MortgageRetention.BasicParameters.FeeAmount != response.FeeAmount
                || offerInstance.MortgageRetention.BasicParameters.FeeAmountDiscounted != response.FeeAmountDiscounted;
    }
}

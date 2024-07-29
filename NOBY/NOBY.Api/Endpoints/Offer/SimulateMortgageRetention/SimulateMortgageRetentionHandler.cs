using DomainServices.CodebookService.Clients;
using DomainServices.OfferService.Clients.v1;

namespace NOBY.Api.Endpoints.Offer.SimulateMortgageRetention;

internal sealed class SimulateMortgageRetentionHandler(
    IOfferServiceClient _offerService, 
    ICodebookServiceClient _codebookService)
        : IRequestHandler<OfferSimulateMortgageRetentionRequest, OfferSimulateMortgageRetentionResponse>
{
    public async Task<OfferSimulateMortgageRetentionResponse> Handle(OfferSimulateMortgageRetentionRequest request, CancellationToken cancellationToken)
    {
        // ziskat int.rate
        var interestRate = await _offerService.GetInterestRate(request.CaseId, request.InterestRateValidFrom, cancellationToken: cancellationToken);
        
        // validace rate
        if (request.InterestRateDiscount.GetValueOrDefault() > 0 && (interestRate - request.InterestRateDiscount!.Value) < 0.1M)
        {
            throw new NobyValidationException(90060);
        }

        var dsRequest = new DomainServices.OfferService.Contracts.SimulateMortgageRetentionRequest
        {
            CaseId = request.CaseId,
            BasicParameters = new()
            {
                FeeAmount = (await _codebookService.FeeChangeRequests(cancellationToken)).First(t => t.IsDefault).Amount,
                FeeAmountDiscounted = request.FeeAmountDiscounted
            },
            SimulationInputs = new()
            {
                InterestRate = interestRate,
                InterestRateDiscount = request.InterestRateDiscount,
                InterestRateValidFrom = request.InterestRateValidFrom
            }
        };

        // spocitat simulaci
        var result = await _offerService.SimulateMortgageRetention(dsRequest, cancellationToken);

        return new()
        {
            OfferId = result.OfferId,
            InterestRate = interestRate,
            InterestRateDiscount = request.InterestRateDiscount,
            InterestRateDiscounted = request.InterestRateDiscount.HasValue ? interestRate - request.InterestRateDiscount.Value : null,
            LoanPaymentAmount = result.SimulationResults.LoanPaymentAmount,
            LoanPaymentAmountDiscounted = result.SimulationResults.LoanPaymentAmountDiscounted,
            FeeAmount = dsRequest.BasicParameters.FeeAmount,
            FeeAmountDiscounted = dsRequest.BasicParameters.FeeAmountDiscounted
        };
    }
}

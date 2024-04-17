using DomainServices.CodebookService.Clients;
using DomainServices.OfferService.Clients.v1;
using NOBY.Dto.Refinancing;

namespace NOBY.Api.Endpoints.Offer.SimulateMortgageRetention;

internal sealed class SimulateMortgageRetentionHandler(
    IOfferServiceClient _offerService, 
    ICodebookServiceClient _codebookService)
        : IRequestHandler<SimulateMortgageRetentionRequest, RefinancingSimulationResult>
{
    public async Task<RefinancingSimulationResult> Handle(SimulateMortgageRetentionRequest request, CancellationToken cancellationToken)
    {
        // ziskat int.rate
        var interestRate = await _offerService.GetInterestRate(request.CaseId, request.InterestRateValidFrom, cancellationToken);

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

        return new RefinancingSimulationResult
        {
            OfferId = result.OfferId,
            InterestRate = interestRate,
            InterestRateDiscount = request.InterestRateDiscount,
            LoanPaymentAmount = result.SimulationResults.LoanPaymentAmount,
            LoanPaymentAmountDiscounted = result.SimulationResults.LoanPaymentAmountDiscounted
        };
    }
}

using DomainServices.CodebookService.Clients;
using DomainServices.OfferService.Clients;

namespace NOBY.Api.Endpoints.Offer.SimulateMortgageRetention;

internal sealed class SimulateMortgageRetentionHandler
    : IRequestHandler<SimulateMortgageRetentionRequest, SimulateMortgageRetentionResponse>
{
    public async Task<SimulateMortgageRetentionResponse> Handle(SimulateMortgageRetentionRequest request, CancellationToken cancellationToken)
    {
        DateTime? interestRateValidFrom = request.InterestRateValidFrom;

        if (interestRateValidFrom.HasValue)
        {
            if (false)
            {
                throw new NobyValidationException(90032, "InterestRateValidFrom is not valid");
            }
        }
        else
        {
            interestRateValidFrom = DateTime.Now;
        }

        // ziskat int.rate
        //await _offerService.GetInterestRate()

        var dsRequest = new DomainServices.OfferService.Contracts.SimulateMortgageRetentionRequest
        {
            CaseId = request.CaseId,
            BasicParameters = new()
            {
                Amount = (await _codebookService.FeeChangeRequests(cancellationToken)).First(t => t.IsDefault).Amount,
                AmountIndividualPrice = request.FeeAmountIndividualPrice
            },
            SimulationInputs = new()
            {
                InterestRate = 1,//GetInterestRate
                InterestRateDiscount = request.InterestRateDiscount,
                InterestRateValidFrom = request.InterestRateValidFrom
            }
        };

        // spocitat simulaci
        var result = await _offerService.SimulateMortgageRetention(dsRequest, cancellationToken);

        return new SimulateMortgageRetentionResponse
        {
            OfferId = result.OfferId,
            LoanPaymentAmount = result.SimulationResults.LoanPaymentAmount,
            LoanPaymentAmountIndividualPrice = result.SimulationResults.LoanPaymentAmountDiscounted
        };
    }

    private readonly ICodebookServiceClient _codebookService;
    private readonly IOfferServiceClient _offerService;

    public SimulateMortgageRetentionHandler(IOfferServiceClient offerService, ICodebookServiceClient codebookService)
    {
        _offerService = offerService;
        _codebookService = codebookService;
    }
}

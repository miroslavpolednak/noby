using DomainServices.OfferService.Clients.v1;
using DomainServices.OfferService.Contracts;

namespace NOBY.Api.Endpoints.Offer.SimulateBuildingSavings;

internal sealed class SimulateBuildingSavingsHandler : IRequestHandler<OfferSimulateBuildingSavingsRequest, OfferSimulateBuildingSavingsResponse>
{
    private readonly IOfferServiceClient _offerService;

    public SimulateBuildingSavingsHandler(IOfferServiceClient offerService)
    {
        _offerService = offerService;
    }

    public async Task<OfferSimulateBuildingSavingsResponse> Handle(OfferSimulateBuildingSavingsRequest request, CancellationToken cancellationToken)
    {
        var response = await _offerService.SimulateBuildingSavings(MapRequestToDomainRequest(request), cancellationToken);

        return MapDomainResponseToResponse(response);
    }

    private static SimulateBuildingSavingsRequest MapRequestToDomainRequest(OfferSimulateBuildingSavingsRequest request)
    {
        return new SimulateBuildingSavingsRequest
        {
            SimulationInputs = new BuildingSavingsSimulationInputs
            {
                TargetAmount = request.TargetAmount,
                MinimumMonthlyDeposit = request.MinimumMonthlyDeposit,
                MarketingActionCode = request.MarketingActionCode,
                ContractStartDate = request.ContractStartDate,
                StateSubsidyRequired = request.StateSubsidyRequired,
                ContractTerminationDate = request.ContractTerminationDate,
                AnnualStatementRequired = request.AnnualStatementRequired,
                IsClientSVJ = request.IsClientSVJ,
                IsClientJuridicalPerson = request.IsClientJuridicalPerson,
                ClientDateOfBirth = request.ClientDateOfBirth,
                SimulateUntilBindingPeriod = request.SimulateUntilBindingPeriod,
                ExtraDeposits =
                {
                    request.ExtraDeposits?.Select(x => new BuildingSavingsExtraDeposit
                    {
                        Date = x.Date,
                        Amount = x.Amount
                    }) ?? []
                }
            }
        };
    }

    private static OfferSimulateBuildingSavingsResponse MapDomainResponseToResponse(SimulateBuildingSavingsResponse response)
    {
        var result = response.SimulationResults;

        return new OfferSimulateBuildingSavingsResponse
        {
            DepositsSum = result.DepositsSum,
            StateSubsidySum = result.StateSubsidySum,
            InterestRate = result.InterestRate,
            BonusInterestRate = result.BonusInterestRate,
            SavingsSum = result.SavingsSum,
            SavingsLengthInMonths = result.SavingsLengthInMonths,
            InterestsSum = result.InterestsSum,
            FeesSum = result.FeesSum,
            InterestBenefitAmount = result.InterestBenefitAmount,
            InterestBenefitTax = result.InterestBenefitTax
        };
    }
}
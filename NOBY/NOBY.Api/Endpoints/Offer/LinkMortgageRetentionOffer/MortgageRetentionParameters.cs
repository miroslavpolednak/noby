using DomainServices.OfferService.Contracts;
using NOBY.Services.OfferLink;

namespace NOBY.Api.Endpoints.Offer.LinkMortgageRetentionOffer;

internal class MortgageRetentionParameters : IMortgageParameters
{
    private readonly MortgageRetentionFullData _retention;

    public MortgageRetentionParameters(MortgageRetentionFullData retention)
    {
        _retention = retention;
    }

    public required long CaseId { get; init; }

    public required long TaskProcessId { get; init; }

    public DateTime InterestRateValidFrom => _retention.SimulationInputs.InterestRateValidFrom;
    public decimal? LoanInterestRateDiscount => (decimal?)_retention.SimulationInputs.InterestRateDiscount == 0 ? null : (decimal?)_retention.SimulationInputs.InterestRateDiscount;
    public decimal LoanInterestRate => _retention.SimulationInputs.InterestRate;
    public decimal? LoanInterestRateProvided => (decimal)_retention.SimulationInputs.InterestRate - LoanInterestRateDiscount;
    public decimal? LoanPaymentAmount => _retention.SimulationResults.LoanPaymentAmount;
    public decimal? LoanPaymentAmountFinal => _retention.SimulationResults.LoanPaymentAmountDiscounted;
    public decimal FeeSum => _retention.BasicParameters.FeeAmount;
    public decimal FeeFinalSum => ((decimal?)_retention.BasicParameters.FeeAmountDiscounted).GetValueOrDefault();
}
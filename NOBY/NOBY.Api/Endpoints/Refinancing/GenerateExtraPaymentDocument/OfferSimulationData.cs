using DomainServices.OfferService.Contracts;

namespace NOBY.Api.Endpoints.Refinancing.GenerateExtraPaymentDocument;

public record OfferSimulationData
{
    public OfferSimulationData(MortgageExtraPaymentSimulationResults simulationResults)
    {
        FeeAmount = simulationResults.FeeAmount;
        PrincipalAmount = simulationResults.PrincipalAmount;
        InterestAmount = simulationResults.InterestAmount;
        OtherUnpaidFees = simulationResults.OtherUnpaidFees;
        InterestOnLate = simulationResults.InterestOnLate;
        InterestCovid = simulationResults.InterestCovid;
        IsLoanOverdue = simulationResults.IsLoanOverdue;
		IsInstallmentReduced = simulationResults.IsInstallmentReduced;
        NewMaturityDate = simulationResults.NewMaturityDate;
        NewPaymentAmount =simulationResults.NewPaymentAmount;
    }

    public decimal FeeAmount { get; }
    
    public decimal PrincipalAmount { get; }

    public decimal InterestAmount { get; }

    public decimal OtherUnpaidFees { get; }

    public decimal InterestOnLate { get; }

    public decimal InterestCovid { get; }

    public bool IsLoanOverdue { get; }

    public bool IsInstallmentReduced { get; }

    public DateTime NewMaturityDate { get; }

    public decimal NewPaymentAmount { get; }
}
namespace NOBY.Services.MortgageRefinancingWorkflow;

public sealed class MortgageRefinancingWorkflowParameters
{
    public long CaseId { get; init; }

    public long TaskProcessId { get; init; }

    public decimal? LoanInterestRateDiscount { get; init; }

    public decimal LoanInterestRate { get; init; }

    public FeeObject? Fee { get; init; }

    public sealed class FeeObject
    {
        public decimal FeeSum { get; set; }

        public decimal FeeFinalSum { get; set; }
    }
}

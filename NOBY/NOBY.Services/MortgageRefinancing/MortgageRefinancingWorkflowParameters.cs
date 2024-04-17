namespace NOBY.Services.MortgageRefinancing;

public sealed class MortgageRefinancingWorkflowParameters
{
    public long CaseId { get; init; }

    public long ProcessId { get; init; }

    public decimal? LoanInterestRateDiscount { get; init; }

    public decimal? LoanInterestRate { get; init; }

    public FeeObject? Fee { get; init; }

    public sealed class FeeObject
    {
        public int FeeId { get; set; }

        public decimal FeeSum { get; set; }

        public decimal FeeFinalSum { get; set; }
    }
}

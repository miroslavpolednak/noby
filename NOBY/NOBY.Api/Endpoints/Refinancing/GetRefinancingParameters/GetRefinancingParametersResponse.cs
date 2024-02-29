using NOBY.Dto.Refinancing;

namespace NOBY.Api.Endpoints.Refinancing.GetRefinancingParameters;

public sealed class GetRefinancingParametersResponse
{
    public bool IsAnotherSalesArrangementInProgress { get; set; }

    public decimal? CustomerPriceSensitivity { get; set; }

    public decimal? CustomerChurnRisk { get; set; }

    public RefinancingParamCurrent RefinancingParametersCurrent { get; set; } = null!;

    public RefinancingParamFuture RefinancingParametersFuture { get; set; } = null!;

    public IReadOnlyCollection<RefinancingProcess> RefinancingProcesses { get; set; } = null!;

}

public sealed class RefinancingParamFuture
{
    public decimal? LoanInterestRate { get; set; }

    public int? FixedRatePeriod { get; set; }

    public decimal? LoanPaymentAmount { get; set; }

    public DateTime? FixedRateValidFrom { get; set; }

    public DateTime? FixedRateValidTo { get; set; }
}

public sealed class RefinancingParamCurrent
{
    public decimal? LoanAmount { get; set; }

    public decimal? Principal { get; set; }

    public DateTime? LoanDueDate { get; set; }

    public decimal? LoanInterestRate { get; set; }

    public int? FixedRatePeriod { get; set; }

    public decimal? LoanPaymentAmount { get; set; }

    public DateTime? FixedRateValidFrom { get; set; }

    public DateTime? FixedRateValidTo { get; set; }
}

public sealed class RefinancingProcess
{
    public int? SalesArrangementId { get; set; }

    public ProcessDetail ProcessDetail { get; set; } = null!;
}

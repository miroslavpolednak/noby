using NOBY.Dto.Refinancing;

namespace NOBY.Api.Endpoints.Refinancing.GetProcessDetail;

public class GetProcessDetailResponse
{
    public ProcessDetail ProcessDetail { get; set; } = null!;

    /// <summary>
    /// Úroková sazba bez slevy
    /// </summary>
    public decimal? LoanInterestRate { get; set; }

    /// <summary>
    /// Splátka bez slevy v Kč
    /// </summary>
    public decimal? LoanPaymentAmount { get; set; }

    /// <summary>
    /// Splátka se slevou v Kč
    /// </summary>
    public decimal? LoanPaymentAmountFinal { get; set; }

    /// <summary>
    /// Poplatek bez slevy v Kč
    /// </summary>
    public decimal? FeeSum { get; set; }

    /// <summary>
    /// Poplatek se slevou v Kč
    /// </summary>
    public decimal? FeeFinalSum { get; set; }
}

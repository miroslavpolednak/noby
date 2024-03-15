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
    public int? LoanPaymentAmount { get; set; }

    /// <summary>
    /// Splátka se slevou v Kč
    /// </summary>
    public int? LoanPaymentAmountFinal { get; set; }

    /// <summary>
    /// Poplatek bez slevy v Kč
    /// </summary>
    public int? FeeSum { get; set; }

    /// <summary>
    /// Poplatek se slevou v Kč
    /// </summary>
    public int? FeeFinalSum { get; set; }
}

using NOBY.Dto.Refinancing;

namespace NOBY.Api.Endpoints.Refinancing.GetProcessDetail;

public class GetProcessDetailResponse
{
    public ProcessDetail ProcessDetail { get; set; } = null!;

    public decimal? LoanInterestRate { get; set; }

    public int? LoanPaymentAmount { get; set; }

    public int? LoanPaymentAmountFinal { get; set; }

    public int? FeeSum { get; set; }

    public int? FeeFinalSum { get; set; }
}

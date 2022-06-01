namespace FOMS.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment;

public class GetLoanApplicationAssessmentResponse
{
    public DateTime RiskBusinesscaseExpirationDate { get; set; }

    public int AssessmentResult { get; set; }

    public List<Dto.AssessmentReason>? Reasons { get; set; }

    public Dto.LoanApplication? Application { get; set; }

    public List<Dto.Household>? Households { get; set; }
}

namespace NOBY.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment;

public class GetLoanApplicationAssessmentResponse
{
    /// <summary>
    /// Datum expirace obchodního případu
    /// </summary>
    public DateTime? RiskBusinesscaseExpirationDate { get; set; }

    /// <summary>
    /// Výsledek vyhodnocení<br/>501 - v pořádku<br/>502 - zamítnuto
    /// </summary>
    public long? AssessmentResult { get; set; }

    /// <summary>
    /// Důvody
    /// </summary>
    public List<Dto.AssessmentReason>? Reasons { get; set; }

    public Dto.LoanApplication? Application { get; set; }

    /// <summary>
    /// Domácnosti
    /// </summary>
    public List<Dto.Household>? Households { get; set; }

    public bool DisplayAssessmentResultInfoText { get; set; }
}

using NOBY.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment.V2.Dto;

namespace NOBY.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment.V2;

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
    public List<AssessmentReason>? Reasons { get; set; }

    public LoanApplication? Application { get; set; }

    /// <summary>
    /// Domácnosti
    /// </summary>
    public List<Dto.Household> Households { get; set; } = null!;

    public bool DisplayAssessmentResultInfoText { get; set; }

    /// <summary>
    /// Informace o tom, zda bylo možné provolat C4M Exposure endpoint. (True=služba Exposure je nedostupná/nepodařilo se jí provolat)
    /// </summary>
    public bool DisplayWarningExposureDoesNotWork { get; set; }
}

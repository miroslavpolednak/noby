namespace FOMS.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment.Dto;

public class AssessmentReason
{
    public string? Code { get; set; }
    public string? Level { get; set; }
    public long? Weight { get; set; }
    public string? Target { get; set; }
    public string? Desc { get; set; }
    public string? Result { get; set; }
}

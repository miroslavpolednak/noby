namespace FOMS.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment;

internal record GetLoanApplicationAssessmentRequest
    : IRequest<GetLoanApplicationAssessmentResponse>
{
    public int SalesArrangementId { get; set; }
}

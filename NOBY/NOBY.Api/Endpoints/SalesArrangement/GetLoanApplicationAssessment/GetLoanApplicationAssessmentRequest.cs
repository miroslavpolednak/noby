namespace NOBY.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment;

internal sealed record GetLoanApplicationAssessmentRequest(int SalesArrangementId, bool NewAssessmentRequired)
    : IRequest<GetLoanApplicationAssessmentResponse>
{
}

namespace NOBY.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment.V1;

internal sealed record GetLoanApplicationAssessmentRequest(int SalesArrangementId, bool NewAssessmentRequired)
    : IRequest<GetLoanApplicationAssessmentResponse>
{
}

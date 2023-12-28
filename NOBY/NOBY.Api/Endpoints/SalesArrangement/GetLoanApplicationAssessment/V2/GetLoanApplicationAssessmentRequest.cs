namespace NOBY.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment.V2;

internal sealed record GetLoanApplicationAssessmentRequest(int SalesArrangementId, bool NewAssessmentRequired)
    : IRequest<GetLoanApplicationAssessmentResponse>
{
}

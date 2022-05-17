namespace FOMS.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment;

internal record GetLoanApplicationAssessmentRequest(int SalesArrangementId)
    : IRequest<GetLoanApplicationAssessmentResponse>
{
}

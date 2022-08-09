﻿namespace FOMS.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment;

internal record GetLoanApplicationAssessmentRequest(int SalesArrangementId, bool NewAssessmentRequired)
    : IRequest<GetLoanApplicationAssessmentResponse>
{
}

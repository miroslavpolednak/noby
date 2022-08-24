using _V2 = DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;
using _sh = DomainServices.RiskIntegrationService.Contracts.Shared.V1;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.RiskBusinessCase.V2.GetAssesment;

internal sealed class GetAssesmentHandler
    : IRequestHandler<_V2.RiskBusinessCaseGetAssesmentRequest, _sh.LoanApplicationAssessmentResponse>
{
    public async Task<_sh.LoanApplicationAssessmentResponse> Handle(_V2.RiskBusinessCaseGetAssesmentRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.GetAssesment(request.LoanApplicationAssessmentId, request.RequestedDetails?.Select(t => t.ToString()).ToList(), cancellationToken);

        return response.ToRIP();
    }

    private readonly Clients.LoanApplicationAssessment.V0_2.ILoanApplicationAssessmentClient _client;

    public GetAssesmentHandler(Clients.LoanApplicationAssessment.V0_2.ILoanApplicationAssessmentClient client)
    {
        _client = client;
    }
}

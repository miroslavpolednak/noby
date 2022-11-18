using _sh = DomainServices.RiskIntegrationService.Contracts.Shared.V1;
using _V2 = DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;
using _cl = DomainServices.RiskIntegrationService.Api.Clients.LoanApplicationAssessment.V1;
using CIS.Core.Configuration;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.RiskBusinessCase.V2.GetAssessment;

internal sealed class GetAssessmentHandler
    : IRequestHandler<_V2.RiskBusinessCaseGetAssessmentRequest, _sh.LoanApplicationAssessmentResponse>
{
    public async Task<_sh.LoanApplicationAssessmentResponse> Handle(_V2.RiskBusinessCaseGetAssessmentRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.GetAssessment(request.LoanApplicationAssessmentId, request.RequestedDetails?.Select(t => t.ToString()).ToList(), cancellationToken);

        return response.ToRIP(_cisEnvironment.EnvironmentName!);
    }

    private readonly _cl.ILoanApplicationAssessmentClient _client;
    private readonly ICisEnvironmentConfiguration _cisEnvironment;

    public GetAssessmentHandler(
        _cl.ILoanApplicationAssessmentClient client,
        ICisEnvironmentConfiguration cisEnvironment)
    {
        _client = client;
        _cisEnvironment = cisEnvironment;
    }
}

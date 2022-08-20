using _V2 = DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;
using _sh = DomainServices.RiskIntegrationService.Contracts.Shared.V1;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.RiskBusinessCase.V2.CreateAssesment;

internal sealed class CreateAssesmentHandler
    : IRequestHandler<_V2.CreateAssesmentRequest, _sh.LoanApplicationAssessmentResponse>
{
    public async Task<_sh.LoanApplicationAssessmentResponse> Handle(_V2.CreateAssesmentRequest request, CancellationToken cancellationToken)
    {
        string chanel = _configuration.GetItChannelFromServiceUser(_serviceUserAccessor.User!.Name);

        var response = await _client.CreateCaseAssessment(request.RiskBusinessCaseId, request.ToC4M(chanel), cancellationToken);

        return response.ToRIP();
    }

    private readonly Clients.RiskBusinessCase.V0_2.IRiskBusinessCaseClient _client;
    private readonly AppConfiguration _configuration;
    private readonly CIS.Core.Security.IServiceUserAccessor _serviceUserAccessor;

    public CreateAssesmentHandler(
        AppConfiguration configuration,
        CIS.Core.Security.IServiceUserAccessor serviceUserAccessor,
        Clients.RiskBusinessCase.V0_2.IRiskBusinessCaseClient client)
    {
        _serviceUserAccessor = serviceUserAccessor;
        _configuration = configuration;
        _client = client;
    }
}

using _V2 = DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;
using _sh = DomainServices.RiskIntegrationService.Contracts.Shared.V1;
using _cl = DomainServices.RiskIntegrationService.ExternalServices.RiskBusinessCase.V1;
using CIS.Core.Configuration;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.RiskBusinessCase.V2.CreateAssessment;

internal sealed class CreateAssessmentHandler
    : IRequestHandler<_V2.RiskBusinessCaseCreateAssessmentRequest, _sh.LoanApplicationAssessmentResponse>
{
    public async Task<_sh.LoanApplicationAssessmentResponse> Handle(_V2.RiskBusinessCaseCreateAssessmentRequest request, CancellationToken cancellationToken)
    {
        string chanel = _configuration.GetItChannelFromServiceUser(_serviceUserAccessor.User!.Name);

        var requestModel = request.ToC4M(chanel, _cisEnvironment.EnvironmentName!);

        var response = await _client.CreateCaseAssessment(request.RiskBusinessCaseId, requestModel, cancellationToken);

        return response.ToRIP(_cisEnvironment.EnvironmentName!);
    }

    private readonly _cl.IRiskBusinessCaseClient _client;
    private readonly AppConfiguration _configuration;
    private readonly CIS.Core.Security.IServiceUserAccessor _serviceUserAccessor;
    private readonly ICisEnvironmentConfiguration _cisEnvironment;

    public CreateAssessmentHandler(
        AppConfiguration configuration,
        CIS.Core.Security.IServiceUserAccessor serviceUserAccessor,
        _cl.IRiskBusinessCaseClient client,
        ICisEnvironmentConfiguration cisEnvironment)
    {
        _serviceUserAccessor = serviceUserAccessor;
        _configuration = configuration;
        _client = client;
        _cisEnvironment = cisEnvironment;
    }
}

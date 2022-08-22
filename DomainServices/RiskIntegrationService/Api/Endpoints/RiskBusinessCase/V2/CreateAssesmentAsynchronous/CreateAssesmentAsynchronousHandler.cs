using _V2 = DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.RiskBusinessCase.V2.CreateAssesmentAsynchronous;

internal sealed class CreateAssesmentAsynchronousHandler
    : IRequestHandler<_V2.RiskBusinessCaseCreateAssesmentAsynchronousRequest, _V2.RiskBusinessCaseCreateAssesmentAsynchronousResponse>
{
    public async Task<_V2.RiskBusinessCaseCreateAssesmentAsynchronousResponse> Handle(_V2.RiskBusinessCaseCreateAssesmentAsynchronousRequest request, CancellationToken cancellationToken)
    {
        string chanel = _configuration.GetItChannelFromServiceUser(_serviceUserAccessor.User!.Name);

        var response = await _client.CreateCaseAssessmentAsynchronous(request.RiskBusinessCaseId, request.ToC4M(chanel), cancellationToken);

        return new ()
        {
            CommandId = response.CommandId
        };
    }

    private readonly Clients.RiskBusinessCase.V0_2.IRiskBusinessCaseClient _client;
    private readonly AppConfiguration _configuration;
    private readonly CIS.Core.Security.IServiceUserAccessor _serviceUserAccessor;

    public CreateAssesmentAsynchronousHandler(
        AppConfiguration configuration,
        CIS.Core.Security.IServiceUserAccessor serviceUserAccessor,
        Clients.RiskBusinessCase.V0_2.IRiskBusinessCaseClient client)
    {
        _serviceUserAccessor = serviceUserAccessor;
        _configuration = configuration;
        _client = client;
    }
}

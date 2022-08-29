using _V2 = DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;
using _cl = DomainServices.RiskIntegrationService.Api.Clients.RiskBusinessCase.V1;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.RiskBusinessCase.V2.CreateAssesmentAsynchronous;

internal sealed class CreateAssesmentAsynchronousHandler
    : IRequestHandler<_V2.RiskBusinessCaseCreateAssesmentAsynchronousRequest, _V2.RiskBusinessCaseCreateAssesmentAsynchronousResponse>
{
    public async Task<_V2.RiskBusinessCaseCreateAssesmentAsynchronousResponse> Handle(_V2.RiskBusinessCaseCreateAssesmentAsynchronousRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException("Verze c4m 0.2 vs 1 je uplne jina. At to nekdo nejdriv popise na confl.");

        string chanel = _configuration.GetItChannelFromServiceUser(_serviceUserAccessor.User!.Name);

        var response = await _client.CreateCaseAssessmentAsynchronous(request.RiskBusinessCaseId, request.ToC4M(chanel), cancellationToken);

        return new();
    }

    private readonly _cl.IRiskBusinessCaseClient _client;
    private readonly AppConfiguration _configuration;
    private readonly CIS.Core.Security.IServiceUserAccessor _serviceUserAccessor;

    public CreateAssesmentAsynchronousHandler(
        AppConfiguration configuration,
        CIS.Core.Security.IServiceUserAccessor serviceUserAccessor,
        _cl.IRiskBusinessCaseClient client)
    {
        _serviceUserAccessor = serviceUserAccessor;
        _configuration = configuration;
        _client = client;
    }
}

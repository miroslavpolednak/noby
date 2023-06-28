using _V2 = DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;
using _C4M = DomainServices.RiskIntegrationService.ExternalServices.RiskBusinessCase.V3.Contracts;
using _cl = DomainServices.RiskIntegrationService.ExternalServices.RiskBusinessCase.V3;
using CIS.Core.Configuration;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.RiskBusinessCase.V2.CreateCase;

internal sealed class CreateCaseHandler
    : IRequestHandler<_V2.RiskBusinessCaseCreateRequest, _V2.RiskBusinessCaseCreateResponse>
{
    public async Task<_V2.RiskBusinessCaseCreateResponse> Handle(_V2.RiskBusinessCaseCreateRequest request, CancellationToken cancellationToken)
    {
        string chanel = _configuration.GetItChannelFromServiceUser(_serviceUserAccessor.User!.Name);

        var requestModel = new _C4M.Create
        {
            ItChannel = FastEnum.Parse<_C4M.ItSubChannelType>(chanel, true),
            LoanApplicationId = _C4M.ResourceIdentifier.CreateLoanApplication(request.SalesArrangementId.ToEnvironmentId(_cisEnvironment.EnvironmentName!), chanel).ToC4M(),
            ResourceProcessId = _C4M.ResourceIdentifier.CreateResourceProcess(request.ResourceProcessId, chanel)?.ToC4M()
        };

        var response = await _client.CreateCase(requestModel, cancellationToken);

        return new _V2.RiskBusinessCaseCreateResponse()
        {
            RiskBusinessCaseId = response.RiskBusinessCaseId
        };
    }

    private readonly _cl.IRiskBusinessCaseClient _client;
    private readonly AppConfiguration _configuration;
    private readonly CIS.Core.Security.IServiceUserAccessor _serviceUserAccessor;
    private readonly ICisEnvironmentConfiguration _cisEnvironment;

    public CreateCaseHandler(
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

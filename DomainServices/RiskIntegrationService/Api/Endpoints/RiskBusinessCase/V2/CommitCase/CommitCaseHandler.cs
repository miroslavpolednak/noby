using DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;
using _V2 = DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.RiskBusinessCase.V2.CommitCase;

internal sealed class CommitCaseHandler
    : IRequestHandler<_V2.RiskBusinessCaseCommitCaseRequest, _V2.RiskBusinessCaseCommitCaseResponse>
{
    public async Task<RiskBusinessCaseCommitCaseResponse> Handle(RiskBusinessCaseCommitCaseRequest request, CancellationToken cancellationToken)
    {
        string chanel = _configuration.GetItChannelFromServiceUser(_serviceUserAccessor.User!.Name);
        var riskApplicationType = Helpers.GetRiskApplicationType(await _codebookService.RiskApplicationTypes(cancellationToken), request.ProductTypeId);




        throw new NotImplementedException();
    }

    private readonly Clients.RiskBusinessCase.V0_2.IRiskBusinessCaseClient _client;
    private readonly AppConfiguration _configuration;
    private readonly CIS.Core.Security.IServiceUserAccessor _serviceUserAccessor;
    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;

    public CommitCaseHandler(
        AppConfiguration configuration,
        CIS.Core.Security.IServiceUserAccessor serviceUserAccessor,
        Clients.RiskBusinessCase.V0_2.IRiskBusinessCaseClient client,
        CodebookService.Abstraction.ICodebookServiceAbstraction codebookService)
    {
        _serviceUserAccessor = serviceUserAccessor;
        _configuration = configuration;
        _client = client;
        _codebookService = codebookService;
    }
}

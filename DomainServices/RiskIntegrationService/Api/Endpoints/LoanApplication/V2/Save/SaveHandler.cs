using _V2 = DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.LoanApplication.V2.Save;

internal sealed class SaveHandler
    : IRequestHandler<_V2.LoanApplicationSaveRequest, _V2.LoanApplicationSaveResponse>
{
    public async Task<_V2.LoanApplicationSaveResponse> Handle(_V2.LoanApplicationSaveRequest request, CancellationToken cancellation)
    {
        // volani c4m
        var response = await _client.Save(null, cancellation);

        return new _V2.LoanApplicationSaveResponse
        {
            RiskSegment = ""
        };
    }

    private readonly CIS.Core.Data.IConnectionProvider<IXxvDapperConnectionProvider> _xxvConnectionProvider;
    private readonly Clients.LoanApplication.V1.ILoanApplicationClient _client;
    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;
    private readonly AppConfiguration _configuration;
    private readonly CIS.Core.Security.IServiceUserAccessor _serviceUserAccessor;

    public SaveHandler(
        AppConfiguration configuration,
        CIS.Core.Security.IServiceUserAccessor serviceUserAccessor,
        Clients.LoanApplication.V1.ILoanApplicationClient client,
        CIS.Core.Data.IConnectionProvider<IXxvDapperConnectionProvider> xxvConnectionProvider,
        CodebookService.Abstraction.ICodebookServiceAbstraction codebookService)
    {
        _serviceUserAccessor = serviceUserAccessor;
        _configuration = configuration;
        _codebookService = codebookService;
        _xxvConnectionProvider = xxvConnectionProvider;
        _client = client;
    }
}

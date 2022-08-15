using _V2 = DomainServices.RiskIntegrationService.Contracts.CustomersExposure.V2;
using _C4M = DomainServices.RiskIntegrationService.Api.Clients.CustomersExposure.V1.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CustomersExposure.V2.Calculate;

internal sealed class CalculateHandler
    : IRequestHandler<_V2.CustomersExposureCalculateRequest, _V2.CustomersExposureCalculateResponse>
{
    public async Task<_V2.CustomersExposureCalculateResponse> Handle(_V2.CustomersExposureCalculateRequest request, CancellationToken cancellation)
    {
        var requestModel = new _C4M.LoanApplicationRelatedExposure
        {
            LoanApplicationId = _C4M.ResourceIdentifier.Create("MPSS", "LA", "LoanApplication", request.CaseId!.ToString(), _configuration.GetItChannelFromServiceUser(_serviceUserAccessor.User!.Name)),
            RiskBusinessCaseId = request.RiskBusinessCaseId,
            LoanApplicationDataVersion = request.LoanApplicationDataVersion
        };

        // human user instance
        if (request.UserIdentity is not null)
        {
            var userInstance = await _xxvConnectionProvider.GetC4mUserInfo(request.UserIdentity, cancellation);
            if (Helpers.IsDealerSchema(request.UserIdentity.IdentityScheme))
                requestModel.LoanApplicationDealer = userInstance.ToC4mDealer(request.UserIdentity);
        }

        var response = await _client.Calculate(requestModel, cancellation);

        var customerRoles = await _codebookService.CustomerRoles(cancellation);

        //return response.ToServiceResponse();
        return null;
    }

    private readonly CIS.Core.Data.IConnectionProvider<IXxvDapperConnectionProvider> _xxvConnectionProvider;
    private readonly Clients.CustomersExposure.V1.ICustomersExposureClient _client;
    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;
    private readonly AppConfiguration _configuration;
    private readonly CIS.Core.Security.IServiceUserAccessor _serviceUserAccessor;

    public CalculateHandler(
        AppConfiguration configuration,
        CIS.Core.Security.IServiceUserAccessor serviceUserAccessor,
        Clients.CustomersExposure.V1.ICustomersExposureClient client,
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

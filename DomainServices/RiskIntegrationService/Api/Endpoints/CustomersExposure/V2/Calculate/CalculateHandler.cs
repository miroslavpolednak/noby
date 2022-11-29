using _V2 = DomainServices.RiskIntegrationService.Contracts.CustomersExposure.V2;
using _C4M = DomainServices.RiskIntegrationService.Api.Clients.CustomersExposure.V1.Contracts;
using System.Globalization;
using CIS.Core.Configuration;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CustomersExposure.V2.Calculate;

internal sealed class CalculateHandler
    : IRequestHandler<_V2.CustomersExposureCalculateRequest, _V2.CustomersExposureCalculateResponse>
{
    public async Task<_V2.CustomersExposureCalculateResponse> Handle(_V2.CustomersExposureCalculateRequest request, CancellationToken cancellation)
    {
        var requestModel = new _C4M.LoanApplicationRelatedExposure
        {
            LoanApplicationId = _C4M.ResourceIdentifier.Create("MPSS", "LA", "LoanApplication", request.SalesArrangementId!.ToEnvironmentId(_cisEnvironment.EnvironmentName!), _configuration.GetItChannelFromServiceUser(_serviceUserAccessor.User!.Name)),
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
        
        // zavolat C4M
        var response = await _client.Calculate(requestModel, cancellation);

        return await response.ToServiceResponse(_codebookService, cancellation);
    }

    private readonly CIS.Core.Data.IConnectionProvider<Data.IXxvDapperConnectionProvider> _xxvConnectionProvider;
    private readonly Clients.CustomersExposure.V1.ICustomersExposureClient _client;
    private readonly CodebookService.Clients.ICodebookServiceClients _codebookService;
    private readonly AppConfiguration _configuration;
    private readonly CIS.Core.Security.IServiceUserAccessor _serviceUserAccessor;
    private readonly ICisEnvironmentConfiguration _cisEnvironment;

    public CalculateHandler(
        AppConfiguration configuration,
        CIS.Core.Security.IServiceUserAccessor serviceUserAccessor,
        Clients.CustomersExposure.V1.ICustomersExposureClient client,
        CIS.Core.Data.IConnectionProvider<Data.IXxvDapperConnectionProvider> xxvConnectionProvider,
        CodebookService.Clients.ICodebookServiceClients codebookService,
        ICisEnvironmentConfiguration cisEnvironment)
    {
        _serviceUserAccessor = serviceUserAccessor;
        _configuration = configuration;
        _codebookService = codebookService;
        _xxvConnectionProvider = xxvConnectionProvider;
        _client = client;
        _cisEnvironment = cisEnvironment;
    }
}

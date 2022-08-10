using _V1 = DomainServices.RiskIntegrationService.Contracts.CustomersExposure.V1;
using _C4M = DomainServices.RiskIntegrationService.Api.Clients.CustomersExposure.V1.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CustomersExposure.V1.Calculate;

internal sealed class CalculateHandler
    : IRequestHandler<_V1.CustomersExposureCalculateRequest, _V1.CustomersExposureCalculateResponse>
{
    public async Task<_V1.CustomersExposureCalculateResponse> Handle(_V1.CustomersExposureCalculateRequest request, CancellationToken cancellation)
    {
        var requestModel = new _C4M.LoanApplicationRelatedExposure
        {
            LoanApplicationId = _C4M.ResourceIdentifier.Create("MPSS", "LA", "LoanApplication", request.CaseId!.ToString(), _configuration.GetItChannelFromServiceUser(_serviceUserAccessor.User!.Name)),
            //RiskBusinessCaseId = request.RiskBusinessCaseId,
            LoanApplicationDataVersion = request.LoanApplicationDataVersion,
        };

        // human user instance
        if (request.UserIdentity is not null)
        {
            var userInstance = await _xxvConnectionProvider.GetC4mUserInfo(request.UserIdentity!.IdentityId, request.UserIdentity.IdentityScheme, cancellation);
            if (!Helpers.IsKbGroupPerson(request.UserIdentity.IdentityScheme))
                requestModel.LoanApplicationDealer = userInstance.ToC4mDealer(request.UserIdentity);
        }

        var response = await _client.Calculate(requestModel, cancellation);

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

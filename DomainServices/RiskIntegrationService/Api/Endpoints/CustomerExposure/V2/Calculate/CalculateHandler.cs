using _V2 = DomainServices.RiskIntegrationService.Contracts.CustomerExposure.V2;
using _C4M = DomainServices.RiskIntegrationService.ExternalServices.CustomerExposure.V3.Contracts;
using _cl = DomainServices.RiskIntegrationService.ExternalServices.CustomerExposure.V3;
using CIS.Core.Configuration;
using CIS.Core.Exceptions;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CustomerExposure.V2.Calculate;

internal sealed class CalculateHandler
    : IRequestHandler<_V2.CustomerExposureCalculateRequest, _V2.CustomerExposureCalculateResponse>
{
    public async Task<_V2.CustomerExposureCalculateResponse> Handle(_V2.CustomerExposureCalculateRequest request, CancellationToken cancellation)
    {
        var requestModel = new _C4M.LoanApplicationRelatedExposure
        {
            LoanApplicationId = _C4M.ResourceIdentifier.Create(request.SalesArrangementId!.ToEnvironmentId(_cisEnvironment.EnvironmentName!), _configuration.GetItChannelFromServiceUser(_serviceUserAccessor.User!.Name)).ToC4M(),
            RiskBusinessCaseId = request.RiskBusinessCaseId,
            LoanApplicationSnapshotId = request.LoanApplicationDataVersion
        };

        // human user instance
        if (request.UserIdentity is not null)
        {
            try
            {
                var userInstance = await _userService.GetUserRIPAttributes(request.UserIdentity.IdentityId ?? "", request.UserIdentity.IdentityScheme ?? "", cancellation);
                if (userInstance != null)
                {
                    if (Helpers.IsDealerSchema(userInstance.DealerCompanyId))
                        requestModel.LoanApplicationDealer = userInstance.ToC4mDealer(request.UserIdentity);
                }
            }
            catch (CisNotFoundException) { }
            catch (Exception)
            {
                throw;
            }
        }
        
        // zavolat C4M
        var response = await _client.Calculate(requestModel, cancellation);

        return await response.ToServiceResponse(_codebookService, cancellation);
    }

    private readonly UserService.Clients.IUserServiceClient _userService;
    private readonly _cl.ICustomerExposureClient _client;
    private readonly CodebookService.Clients.ICodebookServiceClient _codebookService;
    private readonly AppConfiguration _configuration;
    private readonly CIS.Core.Security.IServiceUserAccessor _serviceUserAccessor;
    private readonly ICisEnvironmentConfiguration _cisEnvironment;

    public CalculateHandler(
        AppConfiguration configuration,
        CIS.Core.Security.IServiceUserAccessor serviceUserAccessor,
        _cl.ICustomerExposureClient client,
        CodebookService.Clients.ICodebookServiceClient codebookService,
        ICisEnvironmentConfiguration cisEnvironment,
        UserService.Clients.IUserServiceClient userService)
    {
        _serviceUserAccessor = serviceUserAccessor;
        _configuration = configuration;
        _codebookService = codebookService;
        _client = client;
        _cisEnvironment = cisEnvironment;
        _userService = userService;
    }
}

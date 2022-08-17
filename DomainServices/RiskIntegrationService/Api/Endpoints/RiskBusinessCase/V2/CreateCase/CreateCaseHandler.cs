using _V2 = DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;
using _C4M = DomainServices.RiskIntegrationService.Api.Clients.RiskBusinessCase.V0_2.Contracts;
using DomainServices.RiskIntegrationService.Api.Clients;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.RiskBusinessCase.V2.CreateCase;

internal sealed class CreateCaseHandler
    : IRequestHandler<_V2.CreateCaseRequest, _V2.CreateCaseResponse>
{
    public async Task<_V2.CreateCaseResponse> Handle(_V2.CreateCaseRequest request, CancellationToken cancellationToken)
    {
        string chanel = _configuration.GetItChannelFromServiceUser(_serviceUserAccessor.User!.Name);

        var requestModel = new _C4M.CreateRequest
        {
            ItChannel = FastEnum.Parse<_C4M.CreateRequestItChannel>(chanel),
            LoanApplicationId = new _C4M.ResourceIdentifier
            {
                Id = $"{request.SalesArrangementId}",
                Variant = chanel,
                Instance = Constants.MPSS,
                Domain = Constants.LA,
                Resource = Constants.LoanApplication
            },
            ResourceProcessId = request.ResourceProcessId != null ? new _C4M.ResourceIdentifier
            {
                Id = request.ResourceProcessId,
                Variant = chanel,
                Domain = Constants.OM,
                Instance = Constants.MPSS,
                Resource = Constants.OfferInstance
            } : null
        };

        var response = await _client.CreateCase(requestModel, cancellationToken);

        return new _V2.CreateCaseResponse()
        {
            RiskBusinessCaseId = response.RiskBusinessCaseId.Id
        };
    }

    private readonly Clients.RiskBusinessCase.V0_2.IRiskBusinessCaseClient _client;
    private readonly AppConfiguration _configuration;
    private readonly CIS.Core.Security.IServiceUserAccessor _serviceUserAccessor;

    public CreateCaseHandler(
        AppConfiguration configuration,
        CIS.Core.Security.IServiceUserAccessor serviceUserAccessor,
        Clients.RiskBusinessCase.V0_2.IRiskBusinessCaseClient client)
    {
        _serviceUserAccessor = serviceUserAccessor;
        _configuration = configuration;
        _client = client;
    }
}

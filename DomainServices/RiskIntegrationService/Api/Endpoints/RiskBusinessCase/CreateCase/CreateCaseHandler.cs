using DomainServices.RiskIntegrationService.Api.Clients;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.RiskBusinessCase.CreateCase;

internal sealed class CreateCaseHandler
    : IRequestHandler<Contracts.RiskBusinessCase.CreateCaseRequest, Contracts.RiskBusinessCase.CreateCaseResponse>
{
    public async Task<Contracts.RiskBusinessCase.CreateCaseResponse> Handle(Contracts.RiskBusinessCase.CreateCaseRequest request, CancellationToken cancellation)
    {
        var transformedRequest = new Clients.RiskBusinessCase.V1.Contracts.CreateRequest
        {
            ItChannel = Clients.RiskBusinessCase.V1.Contracts.CreateRequestItChannel.NOBY,
            LoanApplicationId = new Clients.RiskBusinessCase.V1.Contracts.ResourceIdentifier
            {
                Id = request.LoanApplicationIdMp!.Id,
                Variant = request.LoanApplicationIdMp.Name,
                Instance = Constants.MPSS,
                Domain = Constants.LA,
                Resource = Constants.LoanApplication
            },
            ResourceProcessId = request.ResourceProcessIdMp != null ? new Clients.RiskBusinessCase.V1.Contracts.ResourceIdentifier
            {
                Id = request.ResourceProcessIdMp,
                Variant = request.LoanApplicationIdMp.Name,
                Domain = Constants.OM,
                Instance = Constants.MPSS,
                Resource = Constants.OfferInstance
            } : null
        };

        var result = await _client.CreateCase(transformedRequest, cancellation);

        return new Contracts.RiskBusinessCase.CreateCaseResponse
        {
            RiskBusinessCaseIdMp = result.RiskBusinessCaseId.Id
        };
    }

    private readonly Clients.RiskBusinessCase.V1.IRiskBusinessCaseClient _client;

    public CreateCaseHandler(Clients.RiskBusinessCase.V1.IRiskBusinessCaseClient client)
    {
        _client = client;
    }
}

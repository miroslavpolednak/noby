namespace DomainServices.RiskIntegrationService.Api.Endpoints.RiskBusinessCase.V1.Handlers;

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
                Domain = "LA",
                Instance = "MPSS",
                Resource = "LoanApplication",
                Id = request.LoanApplicationIdMp.Id,
                Variant = request.LoanApplicationIdMp.Name
            },
            ResourceProcessId = new Clients.RiskBusinessCase.V1.Contracts.ResourceIdentifier
            {
                Domain = "OM",
                Instance = "MPSS",
                Resource = "OfferInstance",
                Id = request.ResourceProcessIdMp,
                Variant = request.LoanApplicationIdMp.Name
            }
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

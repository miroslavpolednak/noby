namespace DomainServices.RiskIntegrationService.Api.Endpoints.TestService.HalloWorld;

internal class HalloWorldHandler
    : IRequestHandler<Contracts.HalloWorldRequest, Contracts.HalloWorldResponse>
{
    public async Task<Contracts.HalloWorldResponse> Handle(Contracts.HalloWorldRequest request, CancellationToken cancellationToken)
    {
        return new Contracts.HalloWorldResponse { Name = $"My name is {request.Name}" };
    }
}

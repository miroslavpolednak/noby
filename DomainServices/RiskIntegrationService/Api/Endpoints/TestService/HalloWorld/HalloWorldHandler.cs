namespace DomainServices.RiskIntegrationService.Api.Endpoints.TestService.HalloWorld;

public class HalloWorldHandler
    : IRequestHandler<Contracts.HalloWorldRequest, Contracts.HalloWorldResponse>
{
    public Task<Contracts.HalloWorldResponse> Handle(Contracts.HalloWorldRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new Contracts.HalloWorldResponse { Name = $"My name is {request.Name}" });
    }
}

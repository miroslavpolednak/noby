namespace DomainServices.RiskIntegrationService.Contracts;

[ServiceContract(Name = "Test")]
public interface TestEndpoints
{
    ValueTask<HalloWorldResponse> MultiplyAsync(HalloWorldRequest request, CancellationToken cancellationToken = default);
}

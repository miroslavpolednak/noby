namespace DomainServices.RiskIntegrationService.Contracts;

[ServiceContract(Name = "Test")]
public interface ITestService
{
    [OperationContract]
    Task<HalloWorldResponse> HalloWorld(HalloWorldRequest request, CancellationToken cancellationToken = default);
}

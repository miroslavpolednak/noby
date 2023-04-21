using System.ServiceModel;

namespace CIS.Infrastructure.gRPC.CodeFirst;

[ServiceContract(Name = "grpc.health.v1.Health")]
internal interface IGrpcCodeFirstHealthCheck
{
    [OperationContract]
    Task<HealthCheckResponse> Check(HealthCheckRequest request);
}

internal sealed class GrpcCodeFirstHealthCheck : IGrpcCodeFirstHealthCheck
{
    public Task<HealthCheckResponse> Check(HealthCheckRequest request)
    {
        return Task.FromResult(new HealthCheckResponse
        {
            status = ServingStatus.SERVING
        });
    }
}
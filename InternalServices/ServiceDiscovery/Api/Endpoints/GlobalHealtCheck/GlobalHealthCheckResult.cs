namespace CIS.InternalServices.ServiceDiscovery.Api.Endpoints.GlobalHealtCheck;

internal sealed class GlobalHealthCheckResult
{
    public string ServiceName = string.Empty;

    public Grpc.Health.V1.HealthCheckResponse.Types.ServingStatus Status;

    public long ElapsedMilliseconds;

    public string? Description;
}

namespace CIS.Infrastructure.ExternalServicesHelpers;

/// <summary>
/// Marker interface for External Services Clients
/// </summary>
public interface IExternalServiceClient
{
    public string GetVersion() => "";
}

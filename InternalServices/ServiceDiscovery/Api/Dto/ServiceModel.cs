namespace CIS.InternalServices.ServiceDiscovery.Dto;

internal sealed class ServiceModel
{
    public string ServiceName { get; set; } = "";
    public Contracts.ServiceTypes ServiceType {  get; set; }
    public string? ServiceUrl {  get; set; }
}

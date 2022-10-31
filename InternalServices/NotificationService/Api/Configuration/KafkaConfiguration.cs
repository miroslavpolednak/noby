namespace CIS.InternalServices.NotificationService.Api.Configuration;

public class KafkaConfiguration
{
    public string SchemaRegistryUrl { get; set; } = null!;
    public string GroupId { get; set; } = null!;
    public string SslKeystoreLocation { get; set; } = null!;
    public string SslKeystorePassword { get; set; } = null!;
    public string SecurityProtocol { get; set; } = null!;
    public string SslCaLocation { get; set; } = null!;
    public string SslCertificateLocation { get; set; } = null!;
    public string Debug { get; set; } = null!;
    public BootstrapServers BootstrapServers { get; set; } = default!;
}

public class BootstrapServers
{
    public string Business { get; set; } = string.Empty;
    
    public string Logman { get; set; } = string.Empty;
}
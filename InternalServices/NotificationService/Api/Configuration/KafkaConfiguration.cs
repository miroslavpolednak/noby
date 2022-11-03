using Confluent.Kafka;

namespace CIS.InternalServices.NotificationService.Api.Configuration;

public class KafkaConfiguration
{
    public string SchemaRegistryUrl { get; set; } = null!;
    public string GroupId { get; set; } = null!;
    public string Debug { get; set; } = null!;
    public Nodes Nodes { get; set; } = default!;
}

public class Nodes
{
    public Node Business { get; set; } = default!;
    public Node Logman { get; set; } = default!;
}

public class Node
{
    public string BootstrapServers { get; set; } = null!;
    public string SslKeystoreLocation { get; set; } = null!;
    public string SslKeystorePassword { get; set; } = null!;
    public SecurityProtocol? SecurityProtocol { get; set; } = null!;
    public string SslCaLocation { get; set; } = null!;
    public string SslCertificateLocation { get; set; } = null!;
}
using Confluent.Kafka;

namespace CIS.InternalServices.NotificationService.Api.Configuration;

public class KafkaConfiguration
{
    public string GroupId { get; set; } = null!;
    public string Debug { get; set; } = null!;
    public Nodes Nodes { get; set; } = default!;
}

public class Nodes
{
    public Node Business { get; set; } = default!;
}

public class Node
{
    public string BootstrapServers { get; set; } = null!;
    public string SslKeyLocation { get; set; } = null!;
    public string SslKeyPassword { get; set; } = null!;
    public SecurityProtocol? SecurityProtocol { get; set; } = null!;
    public string SslCaLocation { get; set; } = null!;
    public string SslCaCertificateStores { get; set; } = null!;
    public string SslCertificateLocation { get; set; } = null!;
}
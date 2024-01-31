using Confluent.Kafka;

namespace CIS.Infrastructure.Messaging.Configuration;

/// <summary>
/// Konfigurace Kafky - zejmena z Confluent.Kafka.ClientConfig
/// </summary>
public interface IKafkaRiderConfiguration
{
    bool Disabled { get; }

    /// <summary>
    /// https://docs.confluent.io/platform/current/clients/confluent-kafka-dotnet/_site/api/Confluent.Kafka.ClientConfig.html#Confluent_Kafka_ClientConfig_BootstrapServers
    /// </summary>
    string BootstrapServers { get; }

    /// <summary>
    /// https://docs.confluent.io/platform/current/clients/confluent-kafka-dotnet/_site/api/Confluent.Kafka.ClientConfig.html#Confluent_Kafka_ClientConfig_SslKeyLocation
    /// </summary>
    string? SslKeyLocation { get; }

    /// <summary>
    /// https://docs.confluent.io/platform/current/clients/confluent-kafka-dotnet/_site/api/Confluent.Kafka.ClientConfig.html#Confluent_Kafka_ClientConfig_SslKeyPassword
    /// </summary>
    string? SslKeyPassword { get; }

    /// <summary>
    /// https://docs.confluent.io/platform/current/clients/confluent-kafka-dotnet/_site/api/Confluent.Kafka.SecurityProtocol.html
    /// </summary>
    SecurityProtocol SecurityProtocol { get; }

    /// <summary>
    /// https://docs.confluent.io/platform/current/clients/confluent-kafka-dotnet/_site/api/Confluent.Kafka.ClientConfig.html#Confluent_Kafka_ClientConfig_SslCaLocation
    /// </summary>
    string? SslCaLocation { get; }

    /// <summary>
    /// https://docs.confluent.io/platform/current/clients/confluent-kafka-dotnet/_site/api/Confluent.Kafka.ClientConfig.html#Confluent_Kafka_ClientConfig_SslCaCertificateStores
    /// </summary>
    string? SslCaCertificateStores { get; }

    /// <summary>
    /// https://docs.confluent.io/platform/current/clients/confluent-kafka-dotnet/_site/api/Confluent.Kafka.ClientConfig.html#Confluent_Kafka_ClientConfig_SslCertificateLocation
    /// </summary>
    string? SslCertificateLocation { get; }

    /// <summary>
    /// https://docs.confluent.io/platform/current/clients/confluent-kafka-dotnet/_site/api/Confluent.Kafka.ClientConfig.html#Confluent_Kafka_ClientConfig_Debug
    /// </summary>
    string? Debug { get; }

    /// <summary>
    /// https://kafka.apache.org/documentation/#producerconfigs_reconnect.backoff.ms
    /// </summary>
    /// <remarks>Default: 250ms</remarks>
    int ReconnectBackoff { get; }

    /// <summary>
    /// https://kafka.apache.org/documentation/#producerconfigs_reconnect.backoff.max.ms
    /// </summary>
    /// <remarks>Default: 30 minutes</remarks>
    int ReconnectBackoffMaxMinutes { get; }
}

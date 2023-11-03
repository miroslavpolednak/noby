﻿using Confluent.Kafka;

namespace CIS.Infrastructure.Messaging.Configuration;

internal sealed class KafkaRiderConfiguration
    : IKafkaRiderConfiguration
{
    public string BootstrapServers { get; set; } = null!;
    public string? SslKeyLocation { get; set; } = null!;
    public string? SslKeyPassword { get; set; } = null!;
    public SecurityProtocol SecurityProtocol { get; set; } = Confluent.Kafka.SecurityProtocol.Ssl;
    public string? SslCaLocation { get; set; } = null!;
    public string? SslCaCertificateStores { get; set; } = null!;
    public string? SslCertificateLocation { get; set; } = null!;
    public string? Debug { get; set; }
    public int ReconnectBackoff { get; set; } = 250;
    public int ReconnectBackoffMaxMinutes { get; set; } = 30;

    internal void ValidateConfiguration()
    {
        if (string.IsNullOrEmpty(BootstrapServers))
        {
            throw new CIS.Core.Exceptions.CisConfigurationException(0, "CIS.Messaging: BootstrapServers configuration is empty");
        }
    }
}

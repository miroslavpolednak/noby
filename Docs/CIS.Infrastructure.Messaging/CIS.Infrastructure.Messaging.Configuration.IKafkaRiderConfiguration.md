#### [CIS.Infrastructure.Messaging](index.md 'index')
### [CIS.Infrastructure.Messaging.Configuration](CIS.Infrastructure.Messaging.Configuration.md 'CIS.Infrastructure.Messaging.Configuration')

## IKafkaRiderConfiguration Interface

Konfigurace Kafky - zejmena z Confluent.Kafka.ClientConfig

```csharp
public interface IKafkaRiderConfiguration
```
### Properties

<a name='CIS.Infrastructure.Messaging.Configuration.IKafkaRiderConfiguration.BootstrapServers'></a>

## IKafkaRiderConfiguration.BootstrapServers Property

https://docs.confluent.io/platform/current/clients/confluent-kafka-dotnet/_site/api/Confluent.Kafka.ClientConfig.html#Confluent_Kafka_ClientConfig_BootstrapServers

```csharp
string BootstrapServers { get; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

<a name='CIS.Infrastructure.Messaging.Configuration.IKafkaRiderConfiguration.Debug'></a>

## IKafkaRiderConfiguration.Debug Property

https://docs.confluent.io/platform/current/clients/confluent-kafka-dotnet/_site/api/Confluent.Kafka.ClientConfig.html#Confluent_Kafka_ClientConfig_Debug

```csharp
string? Debug { get; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

<a name='CIS.Infrastructure.Messaging.Configuration.IKafkaRiderConfiguration.ReconnectBackoff'></a>

## IKafkaRiderConfiguration.ReconnectBackoff Property

https://kafka.apache.org/documentation/#producerconfigs_reconnect.backoff.ms

```csharp
int ReconnectBackoff { get; }
```

#### Property Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

### Remarks
Default: 250ms

<a name='CIS.Infrastructure.Messaging.Configuration.IKafkaRiderConfiguration.ReconnectBackoffMaxMinutes'></a>

## IKafkaRiderConfiguration.ReconnectBackoffMaxMinutes Property

https://kafka.apache.org/documentation/#producerconfigs_reconnect.backoff.max.ms

```csharp
int ReconnectBackoffMaxMinutes { get; }
```

#### Property Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

### Remarks
Default: 30 minutes

<a name='CIS.Infrastructure.Messaging.Configuration.IKafkaRiderConfiguration.SecurityProtocol'></a>

## IKafkaRiderConfiguration.SecurityProtocol Property

https://docs.confluent.io/platform/current/clients/confluent-kafka-dotnet/_site/api/Confluent.Kafka.SecurityProtocol.html

```csharp
Confluent.Kafka.SecurityProtocol SecurityProtocol { get; }
```

#### Property Value
[Confluent.Kafka.SecurityProtocol](https://docs.microsoft.com/en-us/dotnet/api/Confluent.Kafka.SecurityProtocol 'Confluent.Kafka.SecurityProtocol')

<a name='CIS.Infrastructure.Messaging.Configuration.IKafkaRiderConfiguration.SslCaCertificateStores'></a>

## IKafkaRiderConfiguration.SslCaCertificateStores Property

https://docs.confluent.io/platform/current/clients/confluent-kafka-dotnet/_site/api/Confluent.Kafka.ClientConfig.html#Confluent_Kafka_ClientConfig_SslCaCertificateStores

```csharp
string? SslCaCertificateStores { get; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

<a name='CIS.Infrastructure.Messaging.Configuration.IKafkaRiderConfiguration.SslCaLocation'></a>

## IKafkaRiderConfiguration.SslCaLocation Property

https://docs.confluent.io/platform/current/clients/confluent-kafka-dotnet/_site/api/Confluent.Kafka.ClientConfig.html#Confluent_Kafka_ClientConfig_SslCaLocation

```csharp
string? SslCaLocation { get; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

<a name='CIS.Infrastructure.Messaging.Configuration.IKafkaRiderConfiguration.SslCertificateLocation'></a>

## IKafkaRiderConfiguration.SslCertificateLocation Property

https://docs.confluent.io/platform/current/clients/confluent-kafka-dotnet/_site/api/Confluent.Kafka.ClientConfig.html#Confluent_Kafka_ClientConfig_SslCertificateLocation

```csharp
string? SslCertificateLocation { get; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

<a name='CIS.Infrastructure.Messaging.Configuration.IKafkaRiderConfiguration.SslKeyLocation'></a>

## IKafkaRiderConfiguration.SslKeyLocation Property

https://docs.confluent.io/platform/current/clients/confluent-kafka-dotnet/_site/api/Confluent.Kafka.ClientConfig.html#Confluent_Kafka_ClientConfig_SslKeyLocation

```csharp
string? SslKeyLocation { get; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

<a name='CIS.Infrastructure.Messaging.Configuration.IKafkaRiderConfiguration.SslKeyPassword'></a>

## IKafkaRiderConfiguration.SslKeyPassword Property

https://docs.confluent.io/platform/current/clients/confluent-kafka-dotnet/_site/api/Confluent.Kafka.ClientConfig.html#Confluent_Kafka_ClientConfig_SslKeyPassword

```csharp
string? SslKeyPassword { get; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')
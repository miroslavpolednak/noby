#### [CIS.Infrastructure.Messaging](index.md 'index')
### [CIS.Infrastructure.Messaging.Kafka](CIS.Infrastructure.Messaging.Kafka.md 'CIS.Infrastructure.Messaging.Kafka')

## ICisMessagingKafkaBuilder Interface

```csharp
public interface ICisMessagingKafkaBuilder
```
### Methods

<a name='CIS.Infrastructure.Messaging.Kafka.ICisMessagingKafkaBuilder.AddConsumer_TConsumer_()'></a>

## ICisMessagingKafkaBuilder.AddConsumer<TConsumer>() Method

Přidání single consumera do pipeline Kafky.

```csharp
CIS.Infrastructure.Messaging.Kafka.ICisMessagingKafkaBuilder AddConsumer<TConsumer>()
    where TConsumer : class, MassTransit.IConsumer;
```
#### Type parameters

<a name='CIS.Infrastructure.Messaging.Kafka.ICisMessagingKafkaBuilder.AddConsumer_TConsumer_().TConsumer'></a>

`TConsumer`

Typ consumera implementující interface IConsumer

#### Returns
[ICisMessagingKafkaBuilder](CIS.Infrastructure.Messaging.Kafka.ICisMessagingKafkaBuilder.md 'CIS.Infrastructure.Messaging.Kafka.ICisMessagingKafkaBuilder')

### Remarks
Pro správné přidání konzumera je nutné ještě zavolat metodu AddConsumerTopic!

<a name='CIS.Infrastructure.Messaging.Kafka.ICisMessagingKafkaBuilder.AddConsumerTopic_TTopicMarker_(string,string)'></a>

## ICisMessagingKafkaBuilder.AddConsumerTopic<TTopicMarker>(string, string) Method

Přidání consumerů do pipeline Kafky. Je to druhý krok po AddConsumer().

```csharp
CIS.Infrastructure.Messaging.Kafka.ICisMessagingKafkaBuilder AddConsumerTopic<TTopicMarker>(string topic, string? groupId=null)
    where TTopicMarker : class, Avro.Specific.ISpecificRecord;
```
#### Type parameters

<a name='CIS.Infrastructure.Messaging.Kafka.ICisMessagingKafkaBuilder.AddConsumerTopic_TTopicMarker_(string,string).TTopicMarker'></a>

`TTopicMarker`

Marker interface označující všechny consumery, kteří patří do skupiny daného topicu.
#### Parameters

<a name='CIS.Infrastructure.Messaging.Kafka.ICisMessagingKafkaBuilder.AddConsumerTopic_TTopicMarker_(string,string).topic'></a>

`topic` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Název topicu ze kterého se budou přijímat zprávy.

<a name='CIS.Infrastructure.Messaging.Kafka.ICisMessagingKafkaBuilder.AddConsumerTopic_TTopicMarker_(string,string).groupId'></a>

`groupId` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Název skupiny v Kafce. Pokud je prázdné, doplňmuje se automaticky název aplikace.

#### Returns
[ICisMessagingKafkaBuilder](CIS.Infrastructure.Messaging.Kafka.ICisMessagingKafkaBuilder.md 'CIS.Infrastructure.Messaging.Kafka.ICisMessagingKafkaBuilder')

### Remarks
Každý consumer avro kontrakt (tj. třída implementující ISpecificRecord) musí implementovat interface TTopicMarker, metoda ho díky tomu sama najde a přidá do pipeline.

<a name='CIS.Infrastructure.Messaging.Kafka.ICisMessagingKafkaBuilder.AddProducers_TTopicMarker_(string)'></a>

## ICisMessagingKafkaBuilder.AddProducers<TTopicMarker>(string) Method

Přidání producerů do pipeline Kafky.

```csharp
CIS.Infrastructure.Messaging.Kafka.ICisMessagingKafkaBuilder AddProducers<TTopicMarker>(string topic)
    where TTopicMarker : class, Avro.Specific.ISpecificRecord;
```
#### Type parameters

<a name='CIS.Infrastructure.Messaging.Kafka.ICisMessagingKafkaBuilder.AddProducers_TTopicMarker_(string).TTopicMarker'></a>

`TTopicMarker`

Marker interface označující všechny producery, kteří patří do skupiny daného topicu.
#### Parameters

<a name='CIS.Infrastructure.Messaging.Kafka.ICisMessagingKafkaBuilder.AddProducers_TTopicMarker_(string).topic'></a>

`topic` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Název topicu do kterého se budou posílat zprávy.

#### Returns
[ICisMessagingKafkaBuilder](CIS.Infrastructure.Messaging.Kafka.ICisMessagingKafkaBuilder.md 'CIS.Infrastructure.Messaging.Kafka.ICisMessagingKafkaBuilder')

### Remarks
Každý producer musí implementovat interface TTopicMarker, metoda ho díky tomu sama najde a přidá do pipeline.

<a name='CIS.Infrastructure.Messaging.Kafka.ICisMessagingKafkaBuilder.AddRiderConfiguration(System.Action_MassTransit.IRiderRegistrationConfigurator_)'></a>

## ICisMessagingKafkaBuilder.AddRiderConfiguration(Action<IRiderRegistrationConfigurator>) Method

Extension point pro přidání další konfigurace do configurator.AddRider()

```csharp
CIS.Infrastructure.Messaging.Kafka.ICisMessagingKafkaBuilder AddRiderConfiguration(System.Action<MassTransit.IRiderRegistrationConfigurator> configuration);
```
#### Parameters

<a name='CIS.Infrastructure.Messaging.Kafka.ICisMessagingKafkaBuilder.AddRiderConfiguration(System.Action_MassTransit.IRiderRegistrationConfigurator_).configuration'></a>

`configuration` [System.Action&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Action-1 'System.Action`1')[MassTransit.IRiderRegistrationConfigurator](https://docs.microsoft.com/en-us/dotnet/api/MassTransit.IRiderRegistrationConfigurator 'MassTransit.IRiderRegistrationConfigurator')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Action-1 'System.Action`1')

#### Returns
[ICisMessagingKafkaBuilder](CIS.Infrastructure.Messaging.Kafka.ICisMessagingKafkaBuilder.md 'CIS.Infrastructure.Messaging.Kafka.ICisMessagingKafkaBuilder')

<a name='CIS.Infrastructure.Messaging.Kafka.ICisMessagingKafkaBuilder.AddRiderKafkaConfiguration(System.Action_MassTransit.IKafkaFactoryConfigurator,MassTransit.IRiderRegistrationContext_)'></a>

## ICisMessagingKafkaBuilder.AddRiderKafkaConfiguration(Action<IKafkaFactoryConfigurator,IRiderRegistrationContext>) Method

Extension point pro přidání další konfigurace do rider.UsingKafka()

```csharp
CIS.Infrastructure.Messaging.Kafka.ICisMessagingKafkaBuilder AddRiderKafkaConfiguration(System.Action<MassTransit.IKafkaFactoryConfigurator,MassTransit.IRiderRegistrationContext> configuration);
```
#### Parameters

<a name='CIS.Infrastructure.Messaging.Kafka.ICisMessagingKafkaBuilder.AddRiderKafkaConfiguration(System.Action_MassTransit.IKafkaFactoryConfigurator,MassTransit.IRiderRegistrationContext_).configuration'></a>

`configuration` [System.Action&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Action-2 'System.Action`2')[MassTransit.IKafkaFactoryConfigurator](https://docs.microsoft.com/en-us/dotnet/api/MassTransit.IKafkaFactoryConfigurator 'MassTransit.IKafkaFactoryConfigurator')[,](https://docs.microsoft.com/en-us/dotnet/api/System.Action-2 'System.Action`2')[MassTransit.IRiderRegistrationContext](https://docs.microsoft.com/en-us/dotnet/api/MassTransit.IRiderRegistrationContext 'MassTransit.IRiderRegistrationContext')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Action-2 'System.Action`2')

#### Returns
[ICisMessagingKafkaBuilder](CIS.Infrastructure.Messaging.Kafka.ICisMessagingKafkaBuilder.md 'CIS.Infrastructure.Messaging.Kafka.ICisMessagingKafkaBuilder')

<a name='CIS.Infrastructure.Messaging.Kafka.ICisMessagingKafkaBuilder.Build()'></a>

## ICisMessagingKafkaBuilder.Build() Method

Napojení Kafky do aplikace. Bez zavolání této metody na konci Messaging builderu nebude Kafka v aplikaci nastavena!!!

```csharp
CIS.Infrastructure.Messaging.ICisMessagingBuilder Build();
```

#### Returns
[CIS.Infrastructure.Messaging.ICisMessagingBuilder](https://docs.microsoft.com/en-us/dotnet/api/CIS.Infrastructure.Messaging.ICisMessagingBuilder 'CIS.Infrastructure.Messaging.ICisMessagingBuilder')
using Avro.Specific;
using MassTransit;

namespace CIS.Infrastructure.Messaging.Kafka;

public interface ICisMessagingKafkaBuilder
{
    /// <summary>
    /// Přidání single consumera do pipeline Kafky.
    /// </summary>
    /// <remarks>
    /// Pro správné přidání konzumera je nutné ještě zavolat metodu AddConsumerTopicAvro nebo AddConsumerTopicJson!
    /// </remarks>
    /// <typeparam name="TConsumer">Typ consumera implementující interface IConsumer</typeparam>
    ICisMessagingKafkaBuilder AddConsumer<TConsumer>()
        where TConsumer : class, IConsumer;

    /// <summary>
    /// Přidání producerů do pipeline Kafky pro AVRO.
    /// </summary>
    /// <remarks>
    /// Každý producer musí implementovat interface TTopicMarker, metoda ho díky tomu sama najde a přidá do pipeline.
    /// </remarks>
    /// <typeparam name="TTopicMarker">Marker interface označující všechny producery, kteří patří do skupiny daného topicu.</typeparam>
    /// <param name="topic">Název topicu do kterého se budou posílat zprávy.</param>
    ICisMessagingKafkaBuilder AddProducerAvro<TTopicMarker>(string topic)
        where TTopicMarker : class, ISpecificRecord;

    /// <summary>
    /// Přidání producerů do pipeline Kafky pro JSON.
    /// </summary>
    /// <remarks>
    /// Každý producer musí implementovat interface TTopicMarker, metoda ho díky tomu sama najde a přidá do pipeline.
    /// </remarks>
    /// <typeparam name="TTopicMarker">Marker interface označující všechny producery, kteří patří do skupiny daného topicu.</typeparam>
    /// <param name="topic">Název topicu do kterého se budou posílat zprávy.</param>
    ICisMessagingKafkaBuilder AddProducerJson<TTopicMarker>(string topic)
        where TTopicMarker : class;
    
    /// <summary>
    /// Přidání consumerů do pipeline Kafky pro AVRO. Je to druhý krok po AddConsumer().
    /// </summary>
    /// <remarks>
    /// Každý consumer avro kontrakt (tj. třída implementující ISpecificRecord) musí implementovat interface TTopicMarker, metoda ho díky tomu sama najde a přidá do pipeline.
    /// </remarks>
    /// <typeparam name="TTopicMarker">Marker interface označující všechny consumery, kteří patří do skupiny daného topicu.</typeparam>
    /// <param name="topic">Název topicu ze kterého se budou přijímat zprávy.</param>
    /// <param name="groupId">Název skupiny v Kafce. Pokud je prázdné, doplňmuje se automaticky název aplikace.</param>
    ICisMessagingKafkaBuilder AddConsumerTopicAvro<TTopicMarker>(string topic, string? groupId = null)
        where TTopicMarker : class, ISpecificRecord;

    /// <summary>
    /// Přidání consumerů do pipeline Kafky pro JSON. Je to druhý krok po AddConsumer().
    /// </summary>
    /// <remarks>
    /// Každý consumer json kontrakt musí implementovat interface TTopicMarker, metoda ho díky tomu sama najde a přidá do pipeline.
    /// </remarks>
    /// <typeparam name="TTopicMarker">Marker interface označující všechny consumery, kteří patří do skupiny daného topicu.</typeparam>
    /// <param name="topic">Název topicu ze kterého se budou přijímat zprávy.</param>
    /// <param name="groupId">Název skupiny v Kafce. Pokud je prázdné, doplňmuje se automaticky název aplikace.</param>
    ICisMessagingKafkaBuilder AddConsumerTopicJson<TTopicMarker>(string topic, string? groupId = null)
        where TTopicMarker : class;
    
    /// <summary>
    /// Extension point pro přidání další konfigurace do configurator.AddRider()
    /// </summary>
    ICisMessagingKafkaBuilder AddRiderConfiguration(Action<IRiderRegistrationConfigurator> configuration);

    /// <summary>
    /// Extension point pro přidání další konfigurace do rider.UsingKafka()
    /// </summary>
    ICisMessagingKafkaBuilder AddRiderKafkaConfiguration(Action<IKafkaFactoryConfigurator, IRiderRegistrationContext> configuration);

    /// <summary>
    /// Napojení Kafky do aplikace. Bez zavolání této metody na konci Messaging builderu nebude Kafka v aplikaci nastavena!!!
    /// </summary>
    ICisMessagingBuilder Build();
}

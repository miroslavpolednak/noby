using Avro.Specific;
using CIS.Infrastructure.Messaging.Kafka.Internals;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CIS.Infrastructure.Messaging.Kafka;

public static class ProducerExtensions
{
    public static IRiderRegistrationConfigurator AddProducers<TMarker, TAvro>(
        this IRiderRegistrationConfigurator rider,
        string topic)
        where TMarker : class, ISpecificRecord
        where TAvro : class, ISpecificRecord, TMarker
        => addProducers<TMarker, TAvro, TAvro, TAvro, TAvro>(rider, 1, topic);

    public static IRiderRegistrationConfigurator AddProducers<TMarker, TAvro1, TAvro2>(
        this IRiderRegistrationConfigurator rider,
        string topic)
        where TMarker : class, ISpecificRecord
        where TAvro1 : class, ISpecificRecord, TMarker
        where TAvro2 : class, ISpecificRecord, TMarker
        => addProducers<TMarker, TAvro1, TAvro2, TAvro1, TAvro1>(rider, 2, topic);

    public static IRiderRegistrationConfigurator AddProducers<TMarker, TAvro1, TAvro2, TAvro3>(
        this IRiderRegistrationConfigurator rider,
        string topic)
        where TMarker : class, ISpecificRecord
        where TAvro1 : class, ISpecificRecord, TMarker
        where TAvro2 : class, ISpecificRecord, TMarker
        where TAvro3 : class, ISpecificRecord, TMarker
        => addProducers<TMarker, TAvro1, TAvro2, TAvro3, TAvro1>(rider, 3, topic);

    public static IRiderRegistrationConfigurator AddProducers<TMarker, TAvro1, TAvro2, TAvro3, TAvro4>(
        this IRiderRegistrationConfigurator rider,
        string topic)
        where TMarker : class, ISpecificRecord
        where TAvro1 : class, ISpecificRecord, TMarker
        where TAvro2 : class, ISpecificRecord, TMarker
        where TAvro3 : class, ISpecificRecord, TMarker
        where TAvro4 : class, ISpecificRecord, TMarker
        => addProducers<TMarker, TAvro1, TAvro2, TAvro3, TAvro4>(rider, 4, topic);

    private static IRiderRegistrationConfigurator addProducers<TMarker, TAvro1, TAvro2, TAvro3, TAvro4>(
        IRiderRegistrationConfigurator rider, 
        int producersCount, 
        string topic)
        where TMarker : class, ISpecificRecord
        where TAvro1 : class, ISpecificRecord, TMarker
        where TAvro2 : class, ISpecificRecord, TMarker
        where TAvro3 : class, ISpecificRecord, TMarker
        where TAvro4 : class, ISpecificRecord, TMarker
    {
        var multipleTypeConfigBuilder = new MultipleTypeConfigBuilder<TMarker>();

        for (int i = 1; i <= producersCount; i++)
        {
#pragma warning disable CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).
            _ = i switch
            {
                1 => addToBuilder<TAvro1>(),
                2 => addToBuilder<TAvro2>(),
                3 => addToBuilder<TAvro3>(),
                4 => addToBuilder<TAvro4>()
            };
#pragma warning restore CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).
        }

        var multipleTypeConfig = multipleTypeConfigBuilder.Build();

        rider.AddProducer<TMarker>(topic, (riderContext, conf) =>
        {
            var schemaRegistryClient = riderContext.GetRequiredService<ISchemaRegistryClient>();
            var originalSerializerConfig = riderContext
                .GetRequiredService<IOptions<KB.Speed.Messaging.Kafka.SerDes.Avro.AvroSerializerConfig>>()
                .Value;
            var serializerConfig = new Confluent.SchemaRegistry.Serdes.AvroSerializerConfig
            {
                SubjectNameStrategy = originalSerializerConfig.SubjectNameStrategy,
                AutoRegisterSchemas = originalSerializerConfig.AutoRegisterSchemas
            };

            var valueSerializer = new MultipleTypeSerializer<TMarker>(multipleTypeConfig, schemaRegistryClient, serializerConfig);
            conf.SetValueSerializer(valueSerializer.AsSyncOverAsync());
        });

        return rider;

        bool addToBuilder<T>()
            where T : class, ISpecificRecord, TMarker
        {
            var avroInstance = Activator.CreateInstance<T>();
            multipleTypeConfigBuilder.AddType<T>(avroInstance.Schema);
            return true;
        }
    }

    private static Confluent.SchemaRegistry.Serdes.AvroSerializerConfig Map(
        this KB.Speed.Messaging.Kafka.SerDes.Avro.AvroSerializerConfig config)
    {
        return new Confluent.SchemaRegistry.Serdes.AvroSerializerConfig
        {
            SubjectNameStrategy = config.SubjectNameStrategy,
            AutoRegisterSchemas = config.AutoRegisterSchemas
        };
    }
}

using System.Text.Json;
using Confluent.Kafka;
using Confluent.Kafka.DependencyInjection;
using Mock_KB_NotificationWorker;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton(typeof(IAsyncDeserializer<>), typeof(JsonSerializer));
        services.AddKafkaClient(new Dictionary<string, string>
        {
            { "bootstrap.servers", "localhost:9092" },
            { "enable.idempotence", "true" },
            { "group.id", "group1" }
        });
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
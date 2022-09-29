using Confluent.Kafka.DependencyInjection;
using Mock_KB_NotificationWorker;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
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
using CIS.InternalServices.NotificationService.Msc.AvroSerializers;
using Confluent.Kafka.DependencyInjection;
using Mock_KB_NotificationWorker;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services
            .AddAvroSerializers()
            .AddKafkaClient(new Dictionary<string, string>
            {
                { "bootstrap.servers", "localhost:9092" },
                { "enable.idempotence", "true" },
                { "group.id", "mock-mcs" }
            });
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
using Confluent.Kafka;
using cz.kb.osbs.mcs.notificationreport.eventapi.v3.report;
using cz.kb.osbs.mcs.sender.sendapi.v4.email;
using Example_MassTransit;
using KB.Speed.MassTransit.DependencyInjection;
using KB.Speed.MassTransit.Kafka;
using KB.Speed.Messaging.Kafka.DependencyInjection;
using KB.Speed.Tracing.Extensions;
using KB.Speed.Tracing.Instrumentations.AspNetCore;
using KB.Speed.Tracing.Instrumentations.HttpClient;
using MassTransit;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var kafkaHost = hostContext.Configuration.GetValue<string>("KafkaHost");
        
        services.AddSpeedTracing(hostContext.Configuration, builder =>
        {
            builder.SetDefaultResourceBuilder()
                .AddDefaultExporter()
                .AddSpeedAspNetInstrumentation()
                .AddSpeedHttpClientInstrumentation()
                .AddMassTransitInstrumentation();
        });
        
        services.AddAvroSerializerConfiguration();
        services.AddAvroDeserializerConfiguration();
        services.AddApicurioSchemaRegistry();
        
        services.AddMassTransit(configurator =>
        {
            configurator.UsingInMemory((context, config) =>
            {
                config.ConfigureEndpoints(context);
            });
            
            configurator.AddRider(rider =>
            {
                // add consumers
                rider.AddConsumer<ResultConsumer>();
                
                // add producers
                rider.AddProducerAvro<SendEmail>(Topics.McsSender);

                rider.UsingKafka((context, k) =>
                {
                    k.SecurityProtocol = SecurityProtocol.Ssl;
                    k.Host(kafkaHost, c =>
                    {
                        c.UseSsl(sslConfig =>
                        {
                            sslConfig.EnableCertificateVerification = true;
                            sslConfig.SslCertificateLocation = "C:\\certs\\kafka\\NOBY.pem";
                            sslConfig.KeyLocation = "C:\\certs\\kafka\\NOBY.key";
                            sslConfig.KeyPassword = "noby-cert";
                            sslConfig.SslCaCertificateStores = "Root,CA,Trust";
                        });
                    });
                    
                    // configure topic mapping
                    k.TopicEndpointAvro<NotificationReport, ResultConsumer>(
                        context,
                        Topics.McsResult,
                        "group",
                        e =>
                        {
                        });
                });
            });
        });
        
        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();
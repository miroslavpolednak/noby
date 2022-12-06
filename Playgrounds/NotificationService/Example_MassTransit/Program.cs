using Confluent.Kafka;
using cz.kb.osbs.mcs.notificationreport.eventapi.v3.report;
using cz.kb.osbs.mcs.sender.sendapi.v4.email;
using Example_MassTransit;
using KB.Speed.MassTransit.Kafka;
using KB.Speed.Messaging.Kafka.DependencyInjection;
using MassTransit;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var kafkaHost = hostContext.Configuration.GetValue<string>("KafkaHost");
        
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
                            sslConfig.SslCaLocation = "C:\\certs\\kafka\\CA_test.pem";
                            sslConfig.SslCertificateLocation = "C:\\certs\\kafka\\NOBY.pem";
                            sslConfig.KeyLocation = "C:\\certs\\kafka\\NOBY.key";
                            sslConfig.KeyPassword = "noby-cert";
                            sslConfig.SslCaCertificateStores = "Root,CA,Trust";
                            // sslConfig.KeystoreLocation = "C:\\certs\\kafka\\NOBY.p12";
                            // sslConfig.KeystorePassword = "noby-cert";
                            
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
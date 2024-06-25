# Messaging - Kafka

Podpora asynchronních front je umístěna v projektu `CIS.Infrastructure.Messaging`. Aktuálně se jedná o jedinou KB podporovanou službu - **Kafka**. Pro komunikaci s Kafkou se využívá projekt [KafkaFlow](https://farfetch.github.io/kafkaflow/), který je postaven nad Confluent Kafka.

V KB nyní existují dvě instance Kafky v neprodukčním prostředí:
- Business Kafka
- Logman Kafka

Logman Kafka je stará instance Kafky, která nyní slouží primárně pro logování a postupně se migruje do Business Kafky. Pro nahlížení do Kafky slouží kukátko AKHQ, případně aplikace [Offset Explorer](https://www.kafkatool.com/download.html).

## AKHQ

- [AKHQ Business Kafka](http://kafkabc-test-akhq.service.ist.consul-nprod.kb.cz:8080/ui/kafka-bc/topic)
- [AKHQ Logman Kafka](http://kafkalogc-test-akhq.service.ist.consul-nprod.kb.cz:8080/ui/logc-consul/topic)

### Nodes

V této sekci AKHQ je seznam nodů v rámci Kafky. Jednotlivé topicy mohou být přidělené k více nodům, které se navíc mohou dynamicky měnit. Je potřeba zajistit prostup na všechny Kafka nody, abychom předešli nedeterministickému chování. V detailu topicu je seznam ID nodů.

Dvojitým kliknutím se přejde do nastavení jednotlivého nodu.

### Topics

V této sekci AKHQ je seznam topiců v Kafce. V horní části je textové políčko pro fulltextové vyhledávání.

Po dvojitém kliknutí na topic se přejde do detailu topicu, kde je 6 záložek:
- `Data` - obsahuje Kafka zprávy
- `Partitions` - slouží pro znázornění vztahů jednotlivých nodů pro daný topic
- `Consumer groups` - obsahuje skupiny konzumentů, jednotlivá skupina obdrží Kafka zprávu jen jednou, konzumenti v dané skupině se střídají o zprávy
- `Configs` - slouží pro konfiguraci topicu
- `ACLS` - obsahuje práva daných `CN certifikátů`
- `Logs` - obsahuje logy

## Certifikáty

Pro připojení do Kafky je potřeba mít `klientský CN certifikát`, kterým se autorizujeme vůči topicům, a potřebujeme důvěřovat `serverovskému kořenovému certifikátu` pro vytvoření zabezpečeného spojení.

Pomocí Java toolu `keytool` lze vygenerovat certifikáty .crt.

Následující příkaz přečte aliasy z .jks souboru:
```
> cd C:\Program Files (x86)\Java\jre8\bin
> keytool -list -keystore "C:\certs\KAFKA-BC.truststore.test.jks"
```

Pro každý alias (v ukázce je "kafka") se vygeneruje pomocí následujícího příkazu .crt certifikáty, které se nainstalují do důvěryhodných kořenových certifikátů.

```
> cd C:\Program Files (x86)\Java\jre8\bin
> keytool -export -alias "kafka" -file "C:\certs\broker1.crt" -keystore "C:\certs\KAFKA-BC.truststore.test.jks"
```

## Schema Registry - Apicurio

Pro registraci schémat slouží schema registry [Apicurio Test](https://test.schema-registry.kbcloud/ui/artifacts). Je potřeba si stáhnout a nainstalovat CA root certifikát (např. přes browser) pro správné fungování.

V KB se používají schémata dvojího typu:
- Avro
- Json

Schémata se identifikují pomocí:
- ID (Skupina + Název), verze
- ContentId
- GlobalId

> pozn. Skupina slouží podobně jako namespace, přičemž je potřeba si dát pozor na pojmenování. V některých případech je schema zaregistrováno v defaultní skupině, nicméně název schématu obsahuje zmíněnou skupinu, což může vést k omylům.

Schémata se dají vygenerovat pomocí nástrojů:
- pro Avro - Avrogen 1.11.1
- pro Json - KB.Speed.Dotnet.Tool.Jsonschema.Generator 0.3.0.1

### Avrogen
[Link ke stažení](https://www.nuget.org/packages/Apache.Avro/1.11.1)

Příklad pro vygenerování C# tříd pomocí avrogen:
```
avrogen -s .\Schema_Stazene_Z_Apicurio.json .
```

### Jsonschema Generator

[Dokumentace](https://speed.kb.cz/documentation/net-sdk/tools/features/static/src/kb.speed.dotnet.tool.jsonschema.generator/)

[Link ke stažení](https://nexus3.kb.cz/repository/nuget-all-v3/KB.Speed.Dotnet.Tool.Jsonschema.Generator/0.3.0.1)

## Příprava projektu pro komunikaci s Kafkou

Ve všech projektech používáme stejný pattern implementace Kafky:

### Adresářová struktura

V rootu aplikace je nutné založit adresář `Messaging`. Pod tímto adresářem se vytvářejí podadresáře pro každý typ zprávy - tyto adresáře mají název typu zprávy.
```
Projekt.Api
 |-- Messaging
      |-- TypZpravy1
           |-- Dto
           |-- TypZpravy1.json
           |-- TypZpravy1.cs
           |-- TypZpravy1Handler.cs 
      |-- TypZpravy2
           |-- Dto
           |-- TypZpravy2.json
           |-- TypZpravy2.cs
           |-- TypZpravy2Handler.cs
... 
```

### Konfigurace Messaging projektu

Messaging, tj. i Kafka implementace, má svou vlastní konfiguraci v *appsettings.json*. Tato konfigurace se nachází v objektu `CisMessaging` v rootu konfiguračního souboru a má následující strukturu:

```json
"CisMessaging": {
  "Kafka": {
    "Brokers": [ "<!-- Kafka servers -->" ],
    "SchemaRegistry": {
      "SchemaRegistryUrl": "<!-- Schema registry URL (https://test.schema-registry.kbcloud) -->",
      "SchemaIdentificationType": "ContentId"
    },
    "RetryPolicy": "Default",
    "LogConsumingMessagePayload": true,
    "Admin": {
      "Broker": "<!-- Kafka server pro telemetry a správu Kafky přes Dashboard (pouze non-PROD) - primárně kafkabc-test-broker.service.ist.consul-nprod.kb.cz:9443 -->",
      "Topic": "<!-- Topic NOBY_DS-PERF_MCS_mock_result-event-priv -->"
    },
    "SslKeyLocation": "",
    "SslKeyPassword": "",
    "SecurityProtocol": "Ssl",
    "SslCertificateLocation": ""
  }
}
```

#### RetryPolicy
RetryPolicy nastavuje, jak se konzument má zachovat, pokud dojde k neošetřené chybě. Aktuálně jsou podporované dvě možnosti:

1. Default - výchozí chování, kdy v případě chyby dojde k opakovanému provolání v určitém intervalu.
2. Durable - funkčnost podobná DeadLetter queue, která vyžaduje SQL databázi. Chybně zpracované zprávy se ukládají a pokračuje se v konzumaci dalších zpráv. Na pozadí běží job, který pravidelně zkouší nezpracovanou zprávu znovu zkonzumovat.

### Napojení na Kafku
Každý topic může produkovat buď AVRO, nebo JSON zprávy. V jednom topic může chodit více druhů zpráv stejného typu (avro/json). 
Při vygenerování zprávy je důležité, aby namespace včetně názvu třídy odpovídal celému názvu, který je uvedený v registru schémat.

Pro konzumaci zprávy je potřeba vytvořit třídu, která implemenujte rozhraní `IMessageHandler<>`, kam se jako parametr dává typ zprávy, kterou má konzumovat.
```csharp
internal class MessageObjectHandler : IMessageHandler<MessageObject>
{
    ...
}
```

Konfigurace Kafky se provádí ve start-upu aplikace. Pro každý topic se specifikuje, jaké handlery má použít.
```csharp
SharedComponents.GrpcServiceBuilder
    .CreateGrpcService(args, typeof(Program))
    .Build((builder, appConfiguration) =>
    {
        builder.AddCisMessaging()
                .AddKafkaFlow(msg =>
                {
                    msg.AddConsumerAvro<CaseStateChangedProcessingCompletedHandler>(appConfiguration.SbWorkflowInputProcessingTopic!);

                    msg.AddConsumerAvro(
                        appConfiguration.MortgageServicingMortgageChangesTopic!,
                        handlers =>
                        {
                            handlers.AddHandler<MortgageInstanceChangedHandler>()
                                    .AddHandler<MortgageApplicationChangedHandler>();
                        });
                });
    })
```

### Produkování zpráv

Pro produkování zpráv do topicu je třeba registrovat Producera s daným typem zprávy, která se má posílat. Jako parametr se předává výchozí topic, kam se zprávy mají posílat. V samotné službě pro publikování zpráv je každopádně možné zadat jiný topic, kam se zpráva má odeslat.
```csharp
builder.AddCisMessaging()
            .AddKafkaFlow(msg =>
            {
                msg.AddProducerAvro<MessageObjectToSend>(defaultTopic);
            });
```

Pro získání producera v objektu stačí přes DI si zavolat objekt s generickým rozhraním `IMessageProducer<>`, kde se specifikuje typ zprávy, co se má posílat.
```csharp
IMessageProducer<MessageToSend> producer
```

Odeslání zprávy se poté provede zavoláním metody `ProduceAsync<>`.
```csharp
// Metodě se předává ID zprávy (nepovinné - může být null) a objekt zprávy. Topic se zde neuvádí a tím se využije výchozí, který se nastavuje konfigurací ve startupu.
await _mcsEmailProducer.ProduceAsync(message.id, message);

// Pokud se má zpráva poslat do jiného topicu, než je výchozí, tak stačí využít string parametr na prvním místě.
await _mcsEmailProducer.ProduceAsync(topicName, message.id, message);
```
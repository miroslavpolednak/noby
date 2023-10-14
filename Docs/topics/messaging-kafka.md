# Messaging - Kafka
Podpora asynchroních front je umístěna v projektu `CIS.Infrastructure.Messaging`.
Aktuálně se jedná o jedinou KB podporovanou službu - **Kafka**.

Pro komunikaci s Kafkou používáme infrastrukturu KB připravenou pro SPEED, jmenovitě `KB.Speed.MassTransit.Kafka`.  
SPEED Nuget používáme hlavně proto, že KB má "specifickou" implementaci Kafky, pro kterou bychom museli vytvářet vlastní infra, kterou nám jinak poskytuje `KB.Speed.MassTransit.Kafka`.
Tento Nuget interně používá **MassTransit** (https://masstransit.io/), tj. naše implementace je vlastně implementací MassTransitu a můžeme vycházet z jeho dokumentace.
---dopsat--- kde stahnout ten nuget

> Ale pokud by se dalo časem toho KB Nugetu zbavit, bylo by to super! Nebylo by nic těžkého to přepsat, chce to jen čas.

V KB nyní existují dvě instance Kafky - ??? Jak se vlastně jmenují? Jaký je mezi nimy rozdíl?
---dopsat---

## Příprava projektu pro komunikaci s Kafkou
Ve všech projektech používáme stejný pattern implementace Kafky:

### Adresářová struktura
V rootu aplikace je nutné založit adresář Messaging. Pod tímto adresářem se vytvářejí podadresáře pro každý typ zprávy - tyto adresáře mají název typu zprávy.

---dopsat--- naming konvence pro pojmenovani souboru pod tema adresarema?
```
Projekt.Api
- Messaging
-- TypZpravy1
-- TypZpravy2
```

### Napojení na MassTransit
K napojení na messaging infrastrukturu dochází během startupu aplikace.

```csharp
SharedComponents.GrpcServiceBuilder
    .CreateGrpcService(args, typeof(Program))
    .Build((builder, appConfiguration) =>
    {
        builder.AddCisMessaging()
            .AddKafka(typeof(Program).Assembly)
            .AddConsumer<Api.Messaging.CaseStateChangedProcessingCompleted.CaseStateChanged_ProcessingCompletedConsumer>()
            //...more consumers
            .AddConsumerTopicAvro<ISbWorkflowInputProcessingEvent>("--topic name--")
            //...more topics
            .Build();
    })
```

### Konfigurace Messaging projektu
Messaging, tj. i Kafka implementace, má svou vlastní konfiguraci v *appsettings.json*.
Tato konfigurace se nachází v objektu `CisMessaging` v rootu konfiguračního souboru a má následující strkuturu:

```json
"CisMessaging": {
    "Kafka": {
        "BootstrapServers": "<!-- add Kafka servers -->",
        "SslKeyLocation": "",
        "SslKeyPassword": "",
        "SecurityProtocol": "Ssl",
        "SslCertificateLocation": "",
        "SslCaCertificateStores": "Root,CA,Trust"
    }
},
```

## KB aktuálně podporuje dva formáty zpráv:
- AVRO
- JSON

### Konzumace / publikování AVRO zpráv
---dopsat---
- avrogen
- kde najit schemata
- jak poznat, ze schema je blbe vygenerovane (mam za to, ze u nekterych kontraktu neco chybelo v definici zpravy a pak nefungovala? nejak se to potom muselo pregenerovat v KB?)

### Konzumace / publikování JSON zpráv
---dopsat---

## Zobrazení a kontrola Kafka zpráv
---dopsat---


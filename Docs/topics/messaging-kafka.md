# Messaging - Kafka
Podpora asynchroních front je umístěna v projektu `CIS.Infrastructure.Messaging`.
Aktuálně se jedná o jedinou KB podporovanou službu - **Kafka**.

Pro komunikaci s Kafkou používáme infrastrukturu KB připravenou pro SPEED, jmenovitě `KB.Speed.MassTransit.Kafka`.  
SPEED Nuget používáme hlavně proto, že KB má "specifickou" implementaci Kafky, pro kterou bychom museli vytvářet vlastní infra, kterou nám jinak poskytuje `KB.Speed.MassTransit.Kafka`.
Tento Nuget interně používá [MassTransit](https://masstransit.io/), tj. naše implementace je vlastně implementací MassTransitu a můžeme vycházet z jeho dokumentace. Balíček je dostupný v [Nexus3 KB](https://nexus3.kb.cz) v repozitáří nuget-all-v3.

> Ale pokud by se dalo časem toho KB Nugetu zbavit, bylo by to super! Nebylo by nic těžkého to přepsat, chce to jen čas. Primárně by šlo o implementaci Avro serializace a deserializace pomocí schema registry

V KB nyní existují dvě instance Kafky v neprodukčním prostředí:
- Business Kafka
- Logman Kafka

Logman kafka je stará instance Kafky, která nyní slouží primárně pro logování a postupně se migruje do Business Kafky.

Pro nahlížení do Kafky slouží kukátko AKHQ, případně aplikace [Offset Explorer](https://www.kafkatool.com/download.html).

## AKHQ

Odkazy na existující:
- [AKHQ Business Kafka](http://kafkabc-test-akhq.service.ist.consul-nprod.kb.cz:8080/ui/kafka-bc/topic)
- [AKHQ Logman Kafka](http://kafkalogc-test-akhq.service.ist.consul-nprod.kb.cz:8080/ui/logc-consul/topic)

### Nodes

V této sekci AKHQ je seznam nodů v rámci Kafky. Jednotlivé topicy mohou být přidelené k více nodům, které se navíc mohou dynamicky měnit. Je potřeba zajistit prostup na všechny Kafka nody, abychom předešly nedeterministickým chování. V detailu topicu je seznam IDček nodů

Dvojitým kliknutím se přejde do nastavení jednotlivého nodu.

### Topics

V této sekci AKHQ je seznam topiců v Kafce. V horní části je textové políčko pro full-textové vyhledávání.

Po dvojitým kliknutí na topic se přejde do detailu topicu, kde je 6 tabů
- `Data` - obsahuje kafka zprávy
- `Partitions` - slouží pro znázornění vztahů jednolivých nodů pro daný topic
- `Consumer groups` - obsahuje groupy konzumentů, jednotlivá groupa obdrží kafka zprávu jen jednou, konzumenti v dané groupě se střídají o zprávy
- `Configs` - slouží pro konfiguraci topicu
- `ACLS` - obsahuje práva daných `CN certifikátů`
- `Logs` - obsahuje logy

## Certifikáty

Pro připojení do Kafky je potřeba mít `klientský CN certifikát`, kterým se autorizujeme vůči topicům a potřebujeme důvěřovat `serverovskému kořenovému certifikátu` pro vytvoření zabezpečeného spojení.

Pomocí java toolu `keytool` lze vygenerovat certifikáty .crt.

Následující příkaz přečtě aliasy z .jks souboru
```
> cd C:\Program Files (x86)\Java\jre8\bin
> keytool -list -keystore "C:\certs\KAFKA-BC.truststore.test.jks"
```

Pro každý alias (v ukázce je "kafka") se vygeneruje pomocí následujícího příkazu .crt certifikáty, které se nainstalují do důvěryhodných kořenových certifikátů.

```
> cd C:\Program Files (x86)\Java\jre8\bin
> keytool -export -alias "kafka" -file "C:\certs\broker1.crt" -keystore "C:\certs\KAFKA-BC.truststore.test.jks"
```

## Schema registry - apicurio

Pro registraci schémat slouží schema registry [Apicurio test](https://test.schema-registry.kbcloud/ui/artifacts)

Je potřeba si stáhnout a nainstalovat CA root certifikát (např. přes browser) pro správné fungování.

V KB se používá schemata dvojího typu
- Avro
- Json

Schemata se identifikují pomocí
- Id (Groupa + Název), verze
- ContentId
- GlobalId

> pozn. Groupa slouží podobně jako namespace, přičemž je potřeba si dát pozor na naming. V některých případech je schema zaregistravané v defaultní groupě, nicméně název schematu obsahuje zmíněnou groupu, což může vést k omylům.*

Schémata se dají vygenerovat pomocí toolů
- pro Avro - Avrogen 1.11.1
- pro Json - KB.Speed.Dotnet.Tool.Jsonschema.Generator 0.3.0.1

### Avrogen
[Link ke stažení](https://www.nuget.org/packages/Apache.Avro/1.11.1)

Příklad pro vygenerování C# tříd
```
avrogen -s .\Schema_Stazene_Z_Apicurio.json .
```

### Jsonschema generator

[Dokumentace](https://speed.kb.cz/documentation/net-sdk/tools/features/static/src/kb.speed.dotnet.tool.jsonschema.generator/)
[Link ke stažení](https://nexus3.kb.cz/repository/nuget-all-v3/KB.Speed.Dotnet.Tool.Jsonschema.Generator/0.3.0.1)

## Příprava projektu pro komunikaci s Kafkou
Ve všech projektech používáme stejný pattern implementace Kafky:

### Adresářová struktura
V rootu aplikace je nutné založit adresář Messaging. Pod tímto adresářem se vytvářejí podadresáře pro každý typ zprávy - tyto adresáře mají název typu zprávy.

---dopsat--- naming konvence pro pojmenovani souboru pod tema adresarema?
```
Projekt.Api
 |-- Messaging
      |-- TypZpravy1
      |-- TypZpravy2
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


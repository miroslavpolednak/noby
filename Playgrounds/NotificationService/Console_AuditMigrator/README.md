# Audit Migrator

Migrace aplikačních logů do auditních logů se provádí v několika iteracích, které na sobě navazují.

### 1. krok - Parsování .log souborů

Aplikace nejdříve vyparsuje `.log` soubory a uloží jednotlivé řádky logu do tabulky `ApplicationLogs`.
Aplikace uloží pouze relevantní řádky pro migraci dat ve standardní struktuře aplikačního logu.
Zprocesované soubory si pak uloží do tabulky `ProcessedFiles`, aby nedocházelo k opakovanému parsování souborů.

### 2. krok - Parsování dat určené pro migraci

Většina dat určené pro migraci se nachází ve sloupci `Message` typu `string` v tabulce `ApplicationLogs`.
Apliakce vytěží specifické data podle `LogType` a uloží do tabulky `MigrationData` do sloupce `Payload` (json string).

```csharp
// Database/Entities/LogType.cs

public enum LogType
{
    ReceivedHttpRequest = 0,
    SendingHttpResponse = 1,
    ProducingToKafka = 2,
    ProducedToKafka = 3,
    CouldNotProduceToKafka = 4,
    ReceivedReport = 5
}
```

### 3. Krok - Groupování migračních dat podle notificationId

Aplikace znovu prochází tabulku `MigrationData` a zgroupuje jednotlivé záznamy podle `notificationId`.
Pokud některá migrační data neobsahují `notificationId` zgroupuje tyto data podle `requestId` a doplní k nim `notificationId`.
Informaci o `notificationId` najde v záznamu s `LogType` = `ProducedToKafka`.

Chybějící `notificationId` doplní především pro `LogType`:
- ReceivedHttpRequest
- ProducingToKafka
- CouldNotProduceToKafka

### 4. Krok - Migrace

Pro každý auditovaný `SmsResult` z databáze `NotificationService` (tj. typ `INSIGN_PROCESS`), který je starší než `2023-10-06 00:00:00`:
- najde migrační data v tabulce `MigrationData`
- vytvoří auditní záznamy podle sloupce `State` v `SmsResult`
- zmigruje podle času do databáze `NOBYAudit` do tabulky `AuditEvents`

```csharp
public enum NotificationState
{
    InProgress = 1,
    Unsent = 2,
    Sent = 3,
    Delivered = 4,
    Invalid = 5,
    Error = 6
}
```

[Vytvoření auditních záznamů](https://jira.kb.cz/browse/HFICH-8267)

- NotificationState `Delivered = 4`
  - 1x `AU_NOBY_012` (http)
  - 1x `AU_NOBY_013` (produced to Kafka)
  - 2x `AU_NOBY_014` (received report stav `Sent`, následně stav `Delivered`)

- NotificationState `Invalid = 5`
  - 1x `AU_NOBY_012` (http)
  - 1x `AU_NOBY_013` (produced to Kafka)
  - 1x `AU_NOBY_014` (received report Invalid request)

- NotificationState `Inprogress = 1`
  - 1x `AU_NOBY_012` (http)
  - 1x `AU_NOBY_013` (produced to Kafka)
  - nevytváříme `AU_NOBY_014` protože nepřišla odpověď z MCS a notifikace je stále ve stavu `Inprogress`

- NotificationState `Unsent = 2`, `Sent = 3`, `Invalid = 5`, `Error = 6`
  - 1x `AU_NOBY_012` (http)
  - 1x `AU_NOBY_013` (produced to Kafka)
  - 1x `AU_NOBY_014` (received report with final state)

    
[Ukázka auditních logů](https://wiki.kb.cz/pages/viewpage.action?pageId=689178372)
- z migračních dat typu `ReceivedHttpRequest` a `SendingHttpResponse` vytvoří auditní log `AU_NOBY_012`
- z migračních dat typu `ProducingToKafka`, `ProducedToKafka` případně `CouldNotProduceToKafka` vytvoří auditní log `AU_NOBY_013`
- z migračních dat typu `ReceivedReport` vytvoří auditní log `AU_NOBY_014`

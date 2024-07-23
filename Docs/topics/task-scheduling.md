# Task scheduling service - správce periodických jobů
Periodické background joby vytváříme ve vlastní, tomu vyhrazené, službě **TaskSchedulingService**.
Služba pracuje s konceptem **Jobu** a **Triggerů**.

- **Job** je C# třída/handler obsahující byznys logiku, která se má provést.
- **Trigger** je nastavení konkrétního času spuštění jobu. Každý job může mít více Triggerů.

Kromě vlastního spouštění jobů vystavuje služba gRPC a REST rozhraní, které umožňuje získat informace o běžících jobech, jejich stavu a historii.

## Koncepce schedulingu
Služba **TaskSchedulingService** běží na více nodech zároveň. 
V jednu chvíli je vždy aktivní pouze jeden node, nody si automaticky zajišťují přepínání z pasivního do aktivního režimu pomocí databázových zámků.
Přepínání aktivního/pasivního režimu zajišťuje `ScheduleInstanceLockStatusService`.

Scheduler běží na všech nodech zároveň, nicméně pouze na aktivním nodu se fakticky spouští joby.
Na pasivních nodech se spouští pouze trigger, job nicméně spouštěn není.

Zároveň je pomocí databázových zámků zajištěno, aby se jeden job nikdy nespustil ve více instancích najednou, tj. garantujeme synchroní spouštění jednoho jobu v rámci více triggerů a instancí.

## Vytvoření job handleru
Job handlery jsou třídy implementující rozhraní `IJob` umístěné v adresáři **Jobs**.
Každý handler má vlastní adresář pojmenovaný podle jobu.

Například pro job **MyJob** vytvoříme třídu **MyJobHandler** v adresáři **/Jobs/MyJob**:

```
[Jobs]
  [MyJob]
    MyJobHandler.cs         // C# logika jobu
    LoggerExtensions.cs     // Případné extension metody pro loging
```

> V handleru funguje standardní DI, tzn. jsou k dispozici klienti všech doménových služeb.

## Konfigurace / založení jobu v databázi
*Joby* a *Triggery* jsou uložené v databázi služby *TaskSchedulingService*.
Přidání nového nebo změna stávajícího Jobu/Triggeru se provádí standardní databázovou migrací.

**Job** je zakládán v tabulce **dbo.ScheduleJob**. 
Každý Job má následující atributy:
- `ScheduleJobId` - ID Jobu (GUID)
- `JobName` - název Jobu
- `JobType` - název C# handleru reprezentujícího Job vč. namespace (např. *CIS.InternalServices.TaskSchedulingService.Api.Jobs.DownloadRdmCodebooks.DownloadRdmCodebooksHandler*)
- `Description` - popis Jobu
- `IsDisabled` - pokud je potřeba, je možné spouštění Jobu vypnout nastavením na TRUE

**Trigger** je zakládán v tabulce **dbo.ScheduleTrigger**.
Každý Trigger má následující atributy:
- `ScheduleTriggerId` - ID Triggeru (GUID)
- `ScheduleJobId` - ID Jobu (GUID) ke kterému Trigger patří
- `TriggerName` - název Triggeru
- `Cron` - nastavení času spuštění v Cron formátu
- `JobData` - JSON string s daty, které se předají handleru při spuštění v parametru `jobData`
- `IsDisabled` - pokud je potřeba, je možné spouštění Trigger vypnout nastavením na TRUE

Informace o historii spouštění jobů je v tabulce **dbo.ScheduleJobStatus**.
Tato tabulka obsahuje i `TraceId`, které umožňuje napárovat konkrétní běh jobu na log eventy.

## Administrační rozhraní
Admininstrační API je dostupné zároveň jako gRPC i Json služba na všechn instancích *TaskSchedulingService*.

Endpointy dostupné na API:

**GET /api/v1/jobs**  
Seznam všech jobů v databázi služby.

**GET /api/v1/triggers**  
Seznam všech triggerů v databázi služby.

**POST /api/v1/job-statuses**  
Historie spuštěných jobů včetně jejich aktuálního statusu a TraceId.

**POST /api/v1/job**  
Manuální spuštění konkrétního jobu. Job se spustí na instanci, na které je volané API.

**PUT /api/v1/scheduler**  
Přenačte nastavení triggerů z databáze pro danou instanci. Pozor, updatuje pouze instanci, na které je daný endpoint volaný!
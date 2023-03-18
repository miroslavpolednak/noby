# Background services / periodické joby
## Popis funkčnosti
Periodické joby jsou postaveny nad base třídou .NET frameworku `BackgroundService`, která se používá pro 
dlouhotrvající úlohy běžící na pozadí.
Pro unifikaci používání perirodických jobů / background service vznikla v projektu **CIS.Infrastructure** v namespace `CIS.Infrastructure.BackgroundServices`
infrastruktura pro jejich registraci, spouštění a obsluhu.

Periodicita spouštění jobu se nastavuje pomocí [Crontab expressions](https://github.com/atifaziz/NCrontab/wiki/Crontab-Expression) v konfiguraci jobu.

Job je obalený try/catch blokem, je tedy bezpečné, když v rámci vykonávání jobu dojde k vyhození výjímky. Taková to vyjímka je odchycena a
zalogována, další periodické vykonávání jobu není výjimkou nějak ovlivněno.  

K jobu přistupujeme úplně stejně jako např. ke kontroleru, funguje zde normálně IOC/DI stejně jako u kontroleru 
(scope je zde per job, stejně jako u kontroleru je per request), tedy pokud je potřeba jiný scope než per job, musíme si ho vytvořit.

## Vytvoření jobu
Vytvoříme třídu, která bude implementovat rozhraní `CIS.Infrastructure.BackgroundServices.ICisBackgroundServiceJob`.  
Příklad:
```csharp
internal sealed class TestJob : ICisBackgroundServiceJob
{
    private readonly TestJobConfiguration _configuration;
 
    public TestJob(TestJobConfiguration configuration)
    {
        _configuration = configuration;
    }
 
    public async Task ExecuteJobAsync(CancellationToken cancellationToken)
    {
        // Do some work
    }
}
```

## Konfigurace jobu
Ke každému jobu je třeba vytvořit konfiguraci, kde každý job má povinnou konfigurační sadu parametrů, která je dána generickým rozhraním  
```
CIS.Infrastructure.BackgroundServiceJob.IPeriodicJobConfiguration
```
, kde jako generický parametr je typ jobu. Další konfigurační parametry 
specifické pro job lze přidávat dle libosti.

Příklad:
```csharp
public class TestJobConfiguration : IPeriodicJobConfiguration<TestJob>
{
    public string SectionName => "TestJobConfiguration";

    public bool ServiceDisabled { get; set; }

    public TimeSpan TickInterval { get; set; } = TimeSpan.FromMinutes(1); //Dafault

    public short SpecificParameterForTestJob { get; set; } = 1000;  //Dafault

}
```

### Přidání konfigurace do appsettings
Do appsettings přidáme sekci, která názvem odpovídá parametru SectionName, tedy v našem případě může konfigurace vypadat následovně:

Příklad:
```json
"BackgroundServices": {
    "TestJob": {
        "CronSchedule": "* * * * *",
        "Disabled": false,
        "CustomConfiguration": {
            "MyCustomProperty": true
        }
    }
}
```

### Registrace jobu při startupu aplikace
Registrace se provádí pomocí extension metody `AddCisBackgroundService<TBackgroundService>()` v namespace `CIS.Infrastructure.StartupExtensions`, kde *TBackgroundService* je typem registrovaného jobu.  
Příklad:
```csharp
builder.AddCisBackgroundService<TestJob>();
```
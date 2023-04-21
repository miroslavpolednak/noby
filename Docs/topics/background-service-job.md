# Periodické joby
## Popis funkčnosti
Periodické joby jsou postaveny nad abstrakcí od microsoftu BackgroundService, která se používá pro 
dlouhotrvající úlohy běžící na pozadí. Pro zjednodušení používání perirodických jobů vznikla na úrovni 
```
CIS.Infrastructure.BackgroundServiceJob 
```
infrastruktura pro jejich registraci, spouštění a obsluhu.

Periodické joby se prvně spustí při startu procesu a následně se jejich spouštění opakuje na základě intervalu uvedeného v povinném 
konfiguračním parametru TickInterval, kde tento interval se počítá od ukončení činnosti jobu, nemůže se tedy stát, že vám poběží jeden job vícekrát zároveň.

Job je obalený try/catch bloken, je tedy bezpečné, když v rámci vykonávání jobu dojde k vyhození výjímky. Taková to vyjímka je odchycena a
zalogována, další periodické vykonávání jobu není výjimkou nějak ovlivněno.  

K jobu přistupujeme úplně stejně jako např. ke kontroleru, funguje zde normálně IOC/DI stejně jako u kontroleru 
(scope je zde per job, stejně jako u kontroleru je per request), tedy pokud je potřeba jiný scope než per job, musíme si ho vytvořit.

## Vytvoření jobu

### Vytvoření samotného jobu
Vyvoříme třídu, která bude implementovat rozhraní 
```
CIS.Infrastructure.BackgroundServiceJob.IPeriodicBackgroundServiceJob
```
Příklad:
```csharp
public class TestJob : IPeriodicBackgroundServiceJob
{
 private readonly TestJobConfiguration _configuration;
 
 public TestJob(TestJobConfiguration configuration)
 {
    _configuration = configuration;
 }
 
 public async Task ExecuteJobAsync(CancellationToken cancellationToken)
 {
  // Do work    
 }
}
```

### Vytvoření konfigurace jobu
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
 "TestJobConfiguration": {
    // Každých 15 sekund (00:15:00 - patnáct minut, 15:00:00 - patnáct hodin atd. viz dokumentace https://learn.microsoft.com/cs-cz/dotnet/standard/base-types/standard-timespan-format-strings)
    "TickInterval": "00:00:15",
    "ServiceDisabled": false,
    "SpecificParameterForTestJob": 100
  }
```

### Registrace jobu
Registrace se provádí přes extension metodu  
```
CIS.Infrastructure.StartupExtensions.CisBackgroundServices.AddCisPeriodicJob
```
Příklad:
```csharp
 builder.AddCisPeriodicJob<TestJob, TestJobConfiguration>();
```

Kde jako první generický parametr uvedeme typ jobu a jako druhý generický parametr typ konfigurace.



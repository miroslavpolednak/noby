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
Jedinou metodou toho rozhraní je `ExecuteJobAsync`, která tedy obsahuje kód jobu.
Do konstruktoru jobu je možné standardně přidat dependency z DI.

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
Ke každému jobu je třeba vytvořit konfiguraci, kde každý job má povinnou konfigurační sadu parametrů.
Konfigurace je vždy v *appsettings.json* v sekci **BackgroundServices**.

Struktura konfigurace v *appsettings.json* je daná rozhraním `CIS.Infrastructure.BackgroundServices.ICisBackgroundServiceConfiguration`, kdy:
- název celého objektu konfigurace je názvem typu implementovaného jobu.
- `CronSchedule` je nastavení periodicity spouštění jobu, viz. [Crontab expressions](https://github.com/atifaziz/NCrontab/wiki/Crontab-Expression).
- `Disabled` pokud je potřeba dočasně job vypnout, je možné tuto vlastnost nastavit na True.
- `CustomerConfiguration` je objekt, který je nepovinný. Pokud job potřebuje pro svůj běh nějakou další konfiguraci, tento objekt ji bude obsahovat.

> Všechny background services - ať už implementující base class z `CIS.Infrastructure.BackgroundServices` nebo vlastní, custom vytvořené služby - musí mít konfiguraci v *appsettings.json* v elementu **BackgroundServices** a v struktuře dané rozhraním `CIS.Infrastructure.BackgroundServices.ICisBackgroundServiceConfiguration`.

Ukázka konfigurace v *appsettings.json* pro job s class name "TestJob":
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

## Registrace jobu při startupu aplikace
Registrace se provádí pomocí extension metody `AddCisBackgroundService<TBackgroundService>()` v namespace `CIS.Infrastructure.StartupExtensions`, kde *TBackgroundService* je typem registrovaného jobu.  

Příklad:
```csharp
builder.AddCisBackgroundService<TestJob>();
```

Existuje také extension metoda pro jednoduchou registraci custom konfigurace daného jobu (pokud je vyžadována).
Jedná se o metodu `AddCisBackgroundServiceCustomConfiguration<TBackgroundService, TConfiguration>`, 
kde *TBackgroundService* je typem implementovaného jobu a *TConfiguration* je typem požadované konfigurace.  
Konfigurace je registrována v ID jako singleton daného typu.

Příklad:
```csharp
class TestJobConfiguration {
    public bool MyCustomProperty { get; set; }
}

// registrace jobu včetně custom konfigurace
builder.AddCisBackgroundService<TestJob>();
// registrace konfigurace do DI
builder.AddCisBackgroundServiceCustomConfiguration<TestJob, TestJobConfiguration>();

...

// Následně je možné vytáhnout instanci konfigurace z DI v konstruktoru jobu
internal sealed class TestJob : ICisBackgroundServiceJob
{
    public TestJob(TestJobConfiguration configuration)
    {
        _configuration = configuration;
    }
}
```

## Umístění jobů v rámci projektu aplikace
Joby umísťujeme do adresáře **BackgroundServices** v rootu aplikace.
Zde má pak každý job vlastní adresář a v tomto adresáři je třída s implementací. 
Implementační třída má vždy suffix **Job**.

Umístění pro job "TestJob":
```
[BackgroundServices]
    [Test]
        TestJob.cs
        TestJobConfiguration.cs
```
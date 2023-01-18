# Logování
Komponenty / helpery pro logování jsou umístěny v projektu `CIS.Infrastructure.Telemetry`.
Pro logování (jako implementaci `ILogger`) používáme **Serilog** (https://serilog.net/).
Vždy se používá pouze instance `ILogger` nebo `ILoggerFactory` z DI - nikdy nevoláme přímo statické metody *Serilogu*.

Nastavení *Serilogu* je společné pro všechny projekty, jedná se o extension metodu do startup aplikace:
```csharp
builder.AddCisLogging()
...
var app = builder.Build();
...
app.UseCisLogging();
```

## Druhy logů

### Standardní log
Jedná se o technické logování - je plně v naší režii, logujeme co a jak uznáme za vhodné. Loguje se instancí `ILogger<>` z DI.

### Auditní logování
Jedná se o byznys log. 
Každý auditní záznam je definován byznys analytikem - definice musí obsahovat jaká akce a kdy se má zalogovat, co má být obsahem logu.
Loguje se instancí `IAuditLogger<>` z DI.

## Konfigurace logování
Konfigurace má dvě části - první část konfiguruje co se má logovat, druhá část konfiguruje kam se má logovat.

### Co se má logovat
Nastavení levelu logování, případně potlačení logování některých knihoven se nastavuje standardní konfigurací Serilogu v *appsettings.json*. Např. takto:
```json
"Serilog": {
    "MinimumLevel": {
        "Default": "Warning",
        "Override": {
            "CIS": "Debug",
            "ExternalServices": "Debug",
            "DomainServices": "Debug",
            "Microsoft.AspNetCore.Hosting.Diagnostics": "Information"
        }
    }
}
```

### Kam se má logovat
Standardně používáme tyto Serilog Sinks: **ApplicationInsights, Seq, File, Console, MSSqlServer**.

Nastavení těchto sinků je společné pro všechny služby, nicméně je možné jednotlivé Sinky zapínat v *appsettings.json*, nastavovat jejich connection string atd.
Konfigurace jednotlivých Sinků je v *appsettings.json* v sekci "**CisTelemetry**". Struktura konfigurace viz. `CIS.Infrastructure.Telemetry.Configuration.CisTelemetryConfiguration`.

```json
  "CisTelemetry": {
    "Logging": {
      "LogType": "Grpc",
      "Application": {
        "UseConsole": true,
        "File": {
          "Path": "r:\\LogFiles\\app\\DS-FAT\\CaseService",
          "Filename": "CaseService.log"
        },
        "Seq": {
          "ServerUrl": "http://localhost:6341"
        }
      },
      "Audit": {
        "File": {
          "Path": "r:\\LogFiles\\app\\DS-FAT\\CaseService",
          "Filename": "Audit-CaseService.log"
        }
      }
    }
  }
```

### Použité Serilog Sinky
**File Sink** - používáme pro export logů do KB Logman. Serilog vytváří soubory na serveru, ty jsou následně zpracovány Filebeatem, parsovány a odeslány do Kafky. 
Kafka je následně pošle do ELKu v KB - tj. do Logmana. Formát souboru je pevně daný a hardcodovaný v projektu.

**Console** - užitečné pouze pro debugování nebo lokální spouštění služeb.

**Seq** - protože do Logmana se skoro nikdo nedostane, používáme na prohlížení logů Seq, který je nainstalovaný na aplikačních serverech. Aktuální adresa Seq instance je http://172.30.35.51:6341/.

## Kontextové informace záznamu v logu
Automaticky přidáváme do kontextu záznamu logu tyto informace:

- **Assembly** - název assembly spuštěné aplikace.  
- **Version** - verze spuštěné aplikace.  
- **CisEnvironment** - aplikační prostředí ve kterém je aplikace spouštěna.  
- **CisAppKey** - název spuštěné aplikace.
- **CisUserId** - viz. níže.

Důležité je správně nastavit **LogType** v konfiguraci logování (v appsettings.json), který ovlivňuje jaký middleware se použije pro nastavení kontextových informací v záznamu logu.
Oba middleware nastavují *Serilog* tak, aby ignoroval Health Check requesty.

**LoggerCisUserGrpcMiddleware**  
*LogType = Grpc*. Do kontextu každého záznamu vloží klíč **CisUserId** s hodnotou v33id z HTTP headeru **mp-user-id**. Zároveň *Serilog* ignoruje requesty na gRPC Reflection.

**LoggerCisUserWebapiMiddleware**  
*LogType = WebApi*. Do kontextu každého záznamu vloží klíč **CisUserId** s hodnotou ID uživatele z **ICurrentUserAccessor.User.Id**. Zároveň *Serilog* loguje pouze requesty s URL začínající na "/api/".

## Automatické logování MediatR requestů v gRPC službách
Registrací logování se do *MediatR* pipeline automaticky přidá `CIS.Infrastructure.CisMediatR.PayloadLoggerBehavior`.
Jedná se handler, který loguje payload všech request a response objektů do kontextu log záznamu pod klíčem **Payload**.

## Automatické logování HTTP requestů v Web Api aplikacích
Pro kompletní logování HTTP requestu používáme standardní extension metodu .NETu `appBuilder.UseHttpLogging()`.

## Jak správně logovat standardní log
Snažíme se logovat podle doporučení pro **High Performance logging** (https://learn.microsoft.com/en-us/dotnet/core/extensions/high-performance-logging).
Kromě (zřejmě zanedbatelného) přínosu v nižší spotřebě zdrojů serveru je hlavní výhodou, že díky extension metodám logujeme stejné události ve všech službách stejně.

V praxi se tedy snažíme vyhnout standardním metodám `ILogger` jako `_logger.LogInformation()`, ale vytváříme vlastní extension metody pro každý druh logovaného eventu.  
Např. máme definovanou událost "Entity already exists" - tj. kdykoliv potřebuji zalogovat, že nějaká entita již existuje, tak použiji příslušnou extension metodu `_logger.EntityAlreadyExist()`.

Existuje mnoho již připravených extension metod v projektu `CIS.Infrastructure.Logging.Extensions`.
Tyto je dobré co nejvíce přepoužívat, abychom vše logovali stejně.
Zároveň pokud je potřeba v daném projektu logovat vlastní události, vytvořím si vlastní delegáty a extension metody pro tyto události.

Výsledkem by mělo být, že nikde v kódu nebude logování defaultními *ILogger* metodami.

# Tracing
Tracing zajišťuje implementace **OpenTelemetry** (https://opentelemetry.io/). Zatím není kam exportovat data, takže není žádná vizualizace requestu.

OT zajišťuje propagaci Trace a Span pomocí standardního Activity API v .NETu napříč všemi službami použitými v daném requestu.
Trace se inicializuje na první aplikaci v systému NOBY - většinou tedy na FE API. 
Pokud se jedná o request z FE API, vrací se po ukončení requestu TraceId v HTTP headeru odpovědi na frontend. Toto je zajištěno middlewarem `CIS.Infrastructure.WebApi.Middleware.TraceIdResponseHeaderMiddleware`.

Nastavení tracingu je pomocí extension metody ve startupu aplikace:
```csharp
builder.AddCisTracing()
```

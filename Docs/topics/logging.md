# Logování
Komponenty / helpery pro logování jsou umístěny v projektu **CIS.Infrastructure.Telemetry**.
Pro logování (jako implementaci ILogger) používáme **Serilog** (https://serilog.net/).
Vždy se používá pouze instance ILogger nebo ILoggerFactory z DI - nikdy nevoláme přímo statické metody Serilogu.

Nastavení Serilogu je společné pro všechny projekty, jedná se o extension metodu do startup aplikace:
```
builder.AddCisLogging()
```

## Používáme dva různé způsoby logování

## Standardní log
Jedná se o technické logování - je plně v naší režii, logujeme co a jak uznáme za vhodné. Loguje se instancí `ILogger<>` z DI.

### Auditní logování
Jedná se o byznys log. 
Každý auditní záznam je definován byznys analytikem - definice musí obsahovat jaká akce se má zalogovat, kdy a co má být obsahem logu.
Loguje se instancí `IAuditLogger<>` z DI.

## Konfigurace logování
Konfigurace má dvě části - první část konfiguruje co se má logovat, druhá část konfiguruje kam se má logovat.

### Co se má logovat
Nastavení levelu logování, případně potlačení logování některých knihoven se nastavuje standardní konfigurací Serilogu v *appsettings.json*. Např. takto:
```
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

```
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

# Tracing

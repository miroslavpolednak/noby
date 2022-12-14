# Konfigurace služeb / aplikací
Aplikace konfigurujeme standardním *appsettings.json* souborem. Dále načítáme proměnné prostředí (viz. níže).

Každý konfigurační soubor by měl obsahovat minimálně tyto sekce:
- **CisTelemetry** - nastavení logování - sinks.
- **CisEnvironmentConfiguration** - základní nastavení ekosystému NOBY/CIS.
- **Serilog** - nastavení úrovně logování.

Volitelně potom může obsahovat tyto sekce:
- **ConnectionStrings** - connection strings na databázi.
- **ExternalServices** - konfigurace služeb třetích stran.
- **AppConfiguration** - custom konfigurace pro aktuální službu.

Při startu aplikace je nutné zavolat extension metodu `builder.AddCisEnvironmentConfiguration()` z projektu `CIS.Infrastructure`.
Ta zajistí kontrolu a načtení základní konfigurace aplikace `ICisEnvironmentConfiguration` a její vložení do DI.
Kdykoliv je poté potřeba zjistit např. pro jaké prostředí je a aplikace puštěna, stačí si z DI vyžádat tento interface.

`ICisEnvironmentConfiguration` obsahuje zejména tyto informace:
- **DefaultApplicationKey** - systémový název spuštěné služby.
- **EnvironmentName** - název aplikačního prostředí, pro které je služba spuštěna.

## Nahrazování hodnot z appsettings.json proměnnými prostředí
`AddCisEnvironmentConfiguration` zároveň nahrazuje data z *appsettings.json* proměnnými prostředí pomocí extension metody `AddCisEnvironmentVariables()`.
Toto používáme pro "schování" citlivých informací (hesel) tak, aby je nešlo přečíst z konfiguračního souboru.

Nepoužíváme standardní `AddEnvironmentVariables()` z .NET frameworku, protože ta načítá proměnné pouze z procesu - my potřebujeme načíst **machine level environment variables**.

Aby se proměnné prostředí načetly, musí být v systému uloženy v **System variables** a musí začínat prefixem aplikačního prostředí `{CIS_environment}_`, pro které se mají použít.
Dále fungují stejně jako při použití `AddEnvironmentVariables()`.  
Cílově by měli proměnné prostředí nastavovat CI/CD pipelines.

Příklad nastavení proměnné:
```
setx DEV_ExternalServices__SbWebApi__V1__Username passw0rd /M
```
Nahradí tento klíč z *appsettings.json*:
```
"ExternalServices": {
  "SbWebApi": {
    "V1": {
      "Username": ""
    }
  }
}
```

#### [CIS.Infrastructure.Telemetry](index.md 'index')
### [CIS.Infrastructure.Telemetry](CIS.Infrastructure.Telemetry.md 'CIS.Infrastructure.Telemetry')

## LoggingExtensions Class

Extension metody do startupu aplikace pro registraci logování.

```csharp
public static class LoggingExtensions
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; LoggingExtensions
### Methods

<a name='CIS.Infrastructure.Telemetry.LoggingExtensions.AddCisLogging(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder)'></a>

## LoggingExtensions.AddCisLogging(this WebApplicationBuilder) Method

Přidává do aplikace logování pomocí Serilogu.

```csharp
public static Microsoft.AspNetCore.Builder.WebApplicationBuilder AddCisLogging(this Microsoft.AspNetCore.Builder.WebApplicationBuilder builder);
```
#### Parameters

<a name='CIS.Infrastructure.Telemetry.LoggingExtensions.AddCisLogging(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder).builder'></a>

`builder` [Microsoft.AspNetCore.Builder.WebApplicationBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.Builder.WebApplicationBuilder 'Microsoft.AspNetCore.Builder.WebApplicationBuilder')

#### Returns
[Microsoft.AspNetCore.Builder.WebApplicationBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.Builder.WebApplicationBuilder 'Microsoft.AspNetCore.Builder.WebApplicationBuilder')

### Remarks
Načte konfiguraci logování z appsettings.json.  
Přidá do DI IAuditLogger pro auditní logování.  
Přidá logování request a response do MediatR pipeline.

<a name='CIS.Infrastructure.Telemetry.LoggingExtensions.CloseAndFlush()'></a>

## LoggingExtensions.CloseAndFlush() Method

Pri ukonceni aplikaci se ujisti, ze vsechny sinky jsou vyprazdnene

```csharp
public static void CloseAndFlush();
```

<a name='CIS.Infrastructure.Telemetry.LoggingExtensions.CreateStartupLogger(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder)'></a>

## LoggingExtensions.CreateStartupLogger(this WebApplicationBuilder) Method

Vytvoreni statickeho loggeru pro logovani startupu aplikace.

```csharp
public static CIS.Infrastructure.Telemetry.IStartupLogger CreateStartupLogger(this Microsoft.AspNetCore.Builder.WebApplicationBuilder builder);
```
#### Parameters

<a name='CIS.Infrastructure.Telemetry.LoggingExtensions.CreateStartupLogger(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder).builder'></a>

`builder` [Microsoft.AspNetCore.Builder.WebApplicationBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.Builder.WebApplicationBuilder 'Microsoft.AspNetCore.Builder.WebApplicationBuilder')

#### Returns
[CIS.Infrastructure.Telemetry.IStartupLogger](https://docs.microsoft.com/en-us/dotnet/api/CIS.Infrastructure.Telemetry.IStartupLogger 'CIS.Infrastructure.Telemetry.IStartupLogger')
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

<a name='CIS.Infrastructure.Telemetry.LoggingExtensions.UseCisLogging(thisMicrosoft.AspNetCore.Builder.IApplicationBuilder)'></a>

## LoggingExtensions.UseCisLogging(this IApplicationBuilder) Method

Podle nastavení v appsettings.json zařazuje middleware pro logování buď gRPC nebo Webapi.

```csharp
public static Microsoft.AspNetCore.Builder.IApplicationBuilder UseCisLogging(this Microsoft.AspNetCore.Builder.IApplicationBuilder webApplication);
```
#### Parameters

<a name='CIS.Infrastructure.Telemetry.LoggingExtensions.UseCisLogging(thisMicrosoft.AspNetCore.Builder.IApplicationBuilder).webApplication'></a>

`webApplication` [Microsoft.AspNetCore.Builder.IApplicationBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.Builder.IApplicationBuilder 'Microsoft.AspNetCore.Builder.IApplicationBuilder')

#### Returns
[Microsoft.AspNetCore.Builder.IApplicationBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.Builder.IApplicationBuilder 'Microsoft.AspNetCore.Builder.IApplicationBuilder')
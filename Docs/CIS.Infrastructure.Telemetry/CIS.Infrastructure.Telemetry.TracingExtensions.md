#### [CIS.Infrastructure.Telemetry](index.md 'index')
### [CIS.Infrastructure.Telemetry](CIS.Infrastructure.Telemetry.md 'CIS.Infrastructure.Telemetry')

## TracingExtensions Class

Extension metody do startupu aplikace pro registraci tracingu.

```csharp
public static class TracingExtensions
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; TracingExtensions
### Methods

<a name='CIS.Infrastructure.Telemetry.TracingExtensions.AddCisTracing(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder,string)'></a>

## TracingExtensions.AddCisTracing(this WebApplicationBuilder, string) Method

Register Open Tracing instrumentation

```csharp
public static Microsoft.AspNetCore.Builder.WebApplicationBuilder AddCisTracing(this Microsoft.AspNetCore.Builder.WebApplicationBuilder builder, string? serviceName=null);
```
#### Parameters

<a name='CIS.Infrastructure.Telemetry.TracingExtensions.AddCisTracing(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder,string).builder'></a>

`builder` [Microsoft.AspNetCore.Builder.WebApplicationBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.Builder.WebApplicationBuilder 'Microsoft.AspNetCore.Builder.WebApplicationBuilder')

<a name='CIS.Infrastructure.Telemetry.TracingExtensions.AddCisTracing(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder,string).serviceName'></a>

`serviceName` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Nazev sluzby, ktery se zobrazi v exporteru. Pokud neni zadano, hleda se v ICisEnvironmentConfiguration[DefaultApplicationKey]

#### Returns
[Microsoft.AspNetCore.Builder.WebApplicationBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.Builder.WebApplicationBuilder 'Microsoft.AspNetCore.Builder.WebApplicationBuilder')
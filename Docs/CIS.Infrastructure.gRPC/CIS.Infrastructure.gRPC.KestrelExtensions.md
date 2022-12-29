#### [CIS.Infrastructure.gRPC](index.md 'index')
### [CIS.Infrastructure.gRPC](CIS.Infrastructure.gRPC.md 'CIS.Infrastructure.gRPC')

## KestrelExtensions Class

Nastavení Kestrel serveru pro gRPC služby.

```csharp
public static class KestrelExtensions
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; KestrelExtensions
### Methods

<a name='CIS.Infrastructure.gRPC.KestrelExtensions.UseKestrelWithCustomConfiguration(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder,string)'></a>

## KestrelExtensions.UseKestrelWithCustomConfiguration(this WebApplicationBuilder, string) Method

Umozni nasatavit kestrel custom konfiguracnim souborem.  
Vychozi nazev pro konfiguracni soubor je "kestrel.json". Soubor musi obsahovat root node "CustomeKestrel", pod kterym je struktura CIS.Core.Configuration.KestrelConfiguration.

```csharp
public static Microsoft.AspNetCore.Builder.WebApplicationBuilder UseKestrelWithCustomConfiguration(this Microsoft.AspNetCore.Builder.WebApplicationBuilder builder, string configurationFilename="kestrel.json");
```
#### Parameters

<a name='CIS.Infrastructure.gRPC.KestrelExtensions.UseKestrelWithCustomConfiguration(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder,string).builder'></a>

`builder` [Microsoft.AspNetCore.Builder.WebApplicationBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.Builder.WebApplicationBuilder 'Microsoft.AspNetCore.Builder.WebApplicationBuilder')

<a name='CIS.Infrastructure.gRPC.KestrelExtensions.UseKestrelWithCustomConfiguration(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder,string).configurationFilename'></a>

`configurationFilename` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

#### Returns
[Microsoft.AspNetCore.Builder.WebApplicationBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.Builder.WebApplicationBuilder 'Microsoft.AspNetCore.Builder.WebApplicationBuilder')
#### [CIS.Infrastructure.Security](index.md 'index')
### [CIS.Infrastructure.Security](CIS.Infrastructure.Security.md 'CIS.Infrastructure.Security')

## StartupExtensions Class

Extension metody do startupu aplikace.

```csharp
public static class StartupExtensions
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; StartupExtensions
### Methods

<a name='CIS.Infrastructure.Security.StartupExtensions.AddCisServiceAuthentication(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder)'></a>

## StartupExtensions.AddCisServiceAuthentication(this WebApplicationBuilder) Method

Prida autentizaci volajici aplikace do aktualni sluzby - pouziva se pouze pro interni sluzby. Autentizace je technickym uzivatelem.  
Konfigurace autentizacniho middleware je v CisSecurity:ServiceAuthentication.

```csharp
public static Microsoft.AspNetCore.Builder.WebApplicationBuilder AddCisServiceAuthentication(this Microsoft.AspNetCore.Builder.WebApplicationBuilder builder);
```
#### Parameters

<a name='CIS.Infrastructure.Security.StartupExtensions.AddCisServiceAuthentication(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder).builder'></a>

`builder` [Microsoft.AspNetCore.Builder.WebApplicationBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.Builder.WebApplicationBuilder 'Microsoft.AspNetCore.Builder.WebApplicationBuilder')

#### Returns
[Microsoft.AspNetCore.Builder.WebApplicationBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.Builder.WebApplicationBuilder 'Microsoft.AspNetCore.Builder.WebApplicationBuilder')

<a name='CIS.Infrastructure.Security.StartupExtensions.UseCisServiceUserContext(thisMicrosoft.AspNetCore.Builder.IApplicationBuilder)'></a>

## StartupExtensions.UseCisServiceUserContext(this IApplicationBuilder) Method

Pridava moznost ziskani instance fyzickeho uzivatele volajiciho sluzbu

```csharp
public static Microsoft.AspNetCore.Builder.IApplicationBuilder UseCisServiceUserContext(this Microsoft.AspNetCore.Builder.IApplicationBuilder builder);
```
#### Parameters

<a name='CIS.Infrastructure.Security.StartupExtensions.UseCisServiceUserContext(thisMicrosoft.AspNetCore.Builder.IApplicationBuilder).builder'></a>

`builder` [Microsoft.AspNetCore.Builder.IApplicationBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.Builder.IApplicationBuilder 'Microsoft.AspNetCore.Builder.IApplicationBuilder')

#### Returns
[Microsoft.AspNetCore.Builder.IApplicationBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.Builder.IApplicationBuilder 'Microsoft.AspNetCore.Builder.IApplicationBuilder')
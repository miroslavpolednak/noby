#### [CIS.Infrastructure.ExternalServicesHelpers](index.md 'index')
### [CIS.Infrastructure.ExternalServicesHelpers](CIS.Infrastructure.ExternalServicesHelpers.md 'CIS.Infrastructure.ExternalServicesHelpers')

## StartupRestExtensions Class

```csharp
public static class StartupRestExtensions
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; StartupRestExtensions
### Methods

<a name='CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.AddExternalServiceRestClient_TClient,TImplementation,TConfiguration_(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder,string,string,System.Action_Microsoft.Extensions.DependencyInjection.IHttpClientBuilder,TConfiguration_)'></a>

## StartupRestExtensions.AddExternalServiceRestClient<TClient,TImplementation,TConfiguration>(this WebApplicationBuilder, string, string, Action<IHttpClientBuilder,TConfiguration>) Method

Založení typed HttpClienta pro implementaci ExternalService.

```csharp
public static Microsoft.Extensions.DependencyInjection.IHttpClientBuilder AddExternalServiceRestClient<TClient,TImplementation,TConfiguration>(this Microsoft.AspNetCore.Builder.WebApplicationBuilder builder, string serviceName, string serviceImplementationVersion, System.Action<Microsoft.Extensions.DependencyInjection.IHttpClientBuilder,TConfiguration>? additionalHandlersRegistration=null)
    where TClient : class
    where TImplementation : class, TClient
    where TConfiguration : class, CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration<TClient>;
```
#### Type parameters

<a name='CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.AddExternalServiceRestClient_TClient,TImplementation,TConfiguration_(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder,string,string,System.Action_Microsoft.Extensions.DependencyInjection.IHttpClientBuilder,TConfiguration_).TClient'></a>

`TClient`

Typ klienta - interface pro danou verzi proxy nad API třetí strany

<a name='CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.AddExternalServiceRestClient_TClient,TImplementation,TConfiguration_(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder,string,string,System.Action_Microsoft.Extensions.DependencyInjection.IHttpClientBuilder,TConfiguration_).TImplementation'></a>

`TImplementation`

Interní implementace TClient interface

<a name='CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.AddExternalServiceRestClient_TClient,TImplementation,TConfiguration_(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder,string,string,System.Action_Microsoft.Extensions.DependencyInjection.IHttpClientBuilder,TConfiguration_).TConfiguration'></a>

`TConfiguration`

Typ konfigurace, který bude pro tohoto TClient vložen do Di
#### Parameters

<a name='CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.AddExternalServiceRestClient_TClient,TImplementation,TConfiguration_(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder,string,string,System.Action_Microsoft.Extensions.DependencyInjection.IHttpClientBuilder,TConfiguration_).builder'></a>

`builder` [Microsoft.AspNetCore.Builder.WebApplicationBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.Builder.WebApplicationBuilder 'Microsoft.AspNetCore.Builder.WebApplicationBuilder')

<a name='CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.AddExternalServiceRestClient_TClient,TImplementation,TConfiguration_(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder,string,string,System.Action_Microsoft.Extensions.DependencyInjection.IHttpClientBuilder,TConfiguration_).serviceName'></a>

`serviceName` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Název konzumované služby třetí strany

<a name='CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.AddExternalServiceRestClient_TClient,TImplementation,TConfiguration_(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder,string,string,System.Action_Microsoft.Extensions.DependencyInjection.IHttpClientBuilder,TConfiguration_).serviceImplementationVersion'></a>

`serviceImplementationVersion` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Verze proxy nad API třetí strany

<a name='CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.AddExternalServiceRestClient_TClient,TImplementation,TConfiguration_(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder,string,string,System.Action_Microsoft.Extensions.DependencyInjection.IHttpClientBuilder,TConfiguration_).additionalHandlersRegistration'></a>

`additionalHandlersRegistration` [System.Action&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Action-2 'System.Action`2')[Microsoft.Extensions.DependencyInjection.IHttpClientBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.DependencyInjection.IHttpClientBuilder 'Microsoft.Extensions.DependencyInjection.IHttpClientBuilder')[,](https://docs.microsoft.com/en-us/dotnet/api/System.Action-2 'System.Action`2')[TConfiguration](CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.md#CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.AddExternalServiceRestClient_TClient,TImplementation,TConfiguration_(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder,string,string,System.Action_Microsoft.Extensions.DependencyInjection.IHttpClientBuilder,TConfiguration_).TConfiguration 'CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.AddExternalServiceRestClient<TClient,TImplementation,TConfiguration>(this Microsoft.AspNetCore.Builder.WebApplicationBuilder, string, string, System.Action<Microsoft.Extensions.DependencyInjection.IHttpClientBuilder,TConfiguration>).TConfiguration')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Action-2 'System.Action`2')

Možnost zaregistrovat další HttpHandlery do pipeline.

#### Returns
[Microsoft.Extensions.DependencyInjection.IHttpClientBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.DependencyInjection.IHttpClientBuilder 'Microsoft.Extensions.DependencyInjection.IHttpClientBuilder')

#### Exceptions

[CIS.Core.Exceptions.CisConfigurationException](https://docs.microsoft.com/en-us/dotnet/api/CIS.Core.Exceptions.CisConfigurationException 'CIS.Core.Exceptions.CisConfigurationException')  
Chyba v konfiguraci služby - např. špatně zadaný typ implementace.

[CIS.Core.Exceptions.CisConfigurationNotFound](https://docs.microsoft.com/en-us/dotnet/api/CIS.Core.Exceptions.CisConfigurationNotFound 'CIS.Core.Exceptions.CisConfigurationNotFound')  
Konfigurace typu TConfiguration pro klíč ES:{serviceName}:{serviceImplementationVersion} nebyla nalezena v sekci ExternalServices v appsettings.json

<a name='CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.AddExternalServicesCorrelationIdForwarding(thisMicrosoft.Extensions.DependencyInjection.IHttpClientBuilder,string)'></a>

## StartupRestExtensions.AddExternalServicesCorrelationIdForwarding(this IHttpClientBuilder, string) Method

Doplňuje do každého requestu Correlation Id z OT.

```csharp
public static Microsoft.Extensions.DependencyInjection.IHttpClientBuilder AddExternalServicesCorrelationIdForwarding(this Microsoft.Extensions.DependencyInjection.IHttpClientBuilder builder, string? headerKey=null);
```
#### Parameters

<a name='CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.AddExternalServicesCorrelationIdForwarding(thisMicrosoft.Extensions.DependencyInjection.IHttpClientBuilder,string).builder'></a>

`builder` [Microsoft.Extensions.DependencyInjection.IHttpClientBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.DependencyInjection.IHttpClientBuilder 'Microsoft.Extensions.DependencyInjection.IHttpClientBuilder')

<a name='CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.AddExternalServicesCorrelationIdForwarding(thisMicrosoft.Extensions.DependencyInjection.IHttpClientBuilder,string).headerKey'></a>

`headerKey` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Klíč v hlavičce, kam se má Id zapsat. Pokud není vyplněno, ne nastavena na "X-Correlation-ID".

#### Returns
[Microsoft.Extensions.DependencyInjection.IHttpClientBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.DependencyInjection.IHttpClientBuilder 'Microsoft.Extensions.DependencyInjection.IHttpClientBuilder')

<a name='CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.AddExternalServicesErrorHandling(thisMicrosoft.Extensions.DependencyInjection.IHttpClientBuilder,string)'></a>

## StartupRestExtensions.AddExternalServicesErrorHandling(this IHttpClientBuilder, string) Method

Přidá do HttpClient try-catch tak, aby se nevraceli výchozí vyjímky, ale jejich CIS ekvivalenty.

```csharp
public static Microsoft.Extensions.DependencyInjection.IHttpClientBuilder AddExternalServicesErrorHandling(this Microsoft.Extensions.DependencyInjection.IHttpClientBuilder builder, string serviceName);
```
#### Parameters

<a name='CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.AddExternalServicesErrorHandling(thisMicrosoft.Extensions.DependencyInjection.IHttpClientBuilder,string).builder'></a>

`builder` [Microsoft.Extensions.DependencyInjection.IHttpClientBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.DependencyInjection.IHttpClientBuilder 'Microsoft.Extensions.DependencyInjection.IHttpClientBuilder')

<a name='CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.AddExternalServicesErrorHandling(thisMicrosoft.Extensions.DependencyInjection.IHttpClientBuilder,string).serviceName'></a>

`serviceName` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Název ExternalServices proxy

#### Returns
[Microsoft.Extensions.DependencyInjection.IHttpClientBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.DependencyInjection.IHttpClientBuilder 'Microsoft.Extensions.DependencyInjection.IHttpClientBuilder')

<a name='CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.AddExternalServicesKbHeaders(thisMicrosoft.Extensions.DependencyInjection.IHttpClientBuilder,string)'></a>

## StartupRestExtensions.AddExternalServicesKbHeaders(this IHttpClientBuilder, string) Method

Prida do kazdeho requestu HttpClienta hlavicky vyzadovane v KB.

```csharp
public static Microsoft.Extensions.DependencyInjection.IHttpClientBuilder AddExternalServicesKbHeaders(this Microsoft.Extensions.DependencyInjection.IHttpClientBuilder builder, string? appComponent=null);
```
#### Parameters

<a name='CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.AddExternalServicesKbHeaders(thisMicrosoft.Extensions.DependencyInjection.IHttpClientBuilder,string).builder'></a>

`builder` [Microsoft.Extensions.DependencyInjection.IHttpClientBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.DependencyInjection.IHttpClientBuilder 'Microsoft.Extensions.DependencyInjection.IHttpClientBuilder')

<a name='CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.AddExternalServicesKbHeaders(thisMicrosoft.Extensions.DependencyInjection.IHttpClientBuilder,string).appComponent'></a>

`appComponent` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Hodnota appComp v hlavičce X-KB-Caller-System-Identity. Pokud není vyplněno, je nastavena na "NOBY".

#### Returns
[Microsoft.Extensions.DependencyInjection.IHttpClientBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.DependencyInjection.IHttpClientBuilder 'Microsoft.Extensions.DependencyInjection.IHttpClientBuilder')
#### [CIS.Infrastructure.ExternalServicesHelpers](index.md 'index')
### [CIS.Infrastructure.ExternalServicesHelpers](CIS.Infrastructure.ExternalServicesHelpers.md 'CIS.Infrastructure.ExternalServicesHelpers')

## HttpHandlersExtensions Class

```csharp
public static class HttpHandlersExtensions
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; HttpHandlersExtensions
### Methods

<a name='CIS.Infrastructure.ExternalServicesHelpers.HttpHandlersExtensions.AddExternalServicesCorrelationIdForwarding(thisMicrosoft.Extensions.DependencyInjection.IHttpClientBuilder,string)'></a>

## HttpHandlersExtensions.AddExternalServicesCorrelationIdForwarding(this IHttpClientBuilder, string) Method

Doplňuje do každého requestu Correlation Id z OT.

```csharp
public static Microsoft.Extensions.DependencyInjection.IHttpClientBuilder AddExternalServicesCorrelationIdForwarding(this Microsoft.Extensions.DependencyInjection.IHttpClientBuilder builder, string? headerKey=null);
```
#### Parameters

<a name='CIS.Infrastructure.ExternalServicesHelpers.HttpHandlersExtensions.AddExternalServicesCorrelationIdForwarding(thisMicrosoft.Extensions.DependencyInjection.IHttpClientBuilder,string).builder'></a>

`builder` [Microsoft.Extensions.DependencyInjection.IHttpClientBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.DependencyInjection.IHttpClientBuilder 'Microsoft.Extensions.DependencyInjection.IHttpClientBuilder')

<a name='CIS.Infrastructure.ExternalServicesHelpers.HttpHandlersExtensions.AddExternalServicesCorrelationIdForwarding(thisMicrosoft.Extensions.DependencyInjection.IHttpClientBuilder,string).headerKey'></a>

`headerKey` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Klíč v hlavičce, kam se má Id zapsat. Pokud není vyplněno, ne nastavena na "X-Correlation-ID".

#### Returns
[Microsoft.Extensions.DependencyInjection.IHttpClientBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.DependencyInjection.IHttpClientBuilder 'Microsoft.Extensions.DependencyInjection.IHttpClientBuilder')

<a name='CIS.Infrastructure.ExternalServicesHelpers.HttpHandlersExtensions.AddExternalServicesErrorHandling(thisMicrosoft.Extensions.DependencyInjection.IHttpClientBuilder,string)'></a>

## HttpHandlersExtensions.AddExternalServicesErrorHandling(this IHttpClientBuilder, string) Method

Přidá do HttpClient try-catch tak, aby se nevraceli výchozí vyjímky, ale jejich CIS ekvivalenty.

```csharp
public static Microsoft.Extensions.DependencyInjection.IHttpClientBuilder AddExternalServicesErrorHandling(this Microsoft.Extensions.DependencyInjection.IHttpClientBuilder builder, string serviceName);
```
#### Parameters

<a name='CIS.Infrastructure.ExternalServicesHelpers.HttpHandlersExtensions.AddExternalServicesErrorHandling(thisMicrosoft.Extensions.DependencyInjection.IHttpClientBuilder,string).builder'></a>

`builder` [Microsoft.Extensions.DependencyInjection.IHttpClientBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.DependencyInjection.IHttpClientBuilder 'Microsoft.Extensions.DependencyInjection.IHttpClientBuilder')

<a name='CIS.Infrastructure.ExternalServicesHelpers.HttpHandlersExtensions.AddExternalServicesErrorHandling(thisMicrosoft.Extensions.DependencyInjection.IHttpClientBuilder,string).serviceName'></a>

`serviceName` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Název ExternalServices proxy

#### Returns
[Microsoft.Extensions.DependencyInjection.IHttpClientBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.DependencyInjection.IHttpClientBuilder 'Microsoft.Extensions.DependencyInjection.IHttpClientBuilder')

<a name='CIS.Infrastructure.ExternalServicesHelpers.HttpHandlersExtensions.AddExternalServicesKbHeaders(thisMicrosoft.Extensions.DependencyInjection.IHttpClientBuilder,string)'></a>

## HttpHandlersExtensions.AddExternalServicesKbHeaders(this IHttpClientBuilder, string) Method

Prida do kazdeho requestu HttpClienta hlavicky vyzadovane v KB.

```csharp
public static Microsoft.Extensions.DependencyInjection.IHttpClientBuilder AddExternalServicesKbHeaders(this Microsoft.Extensions.DependencyInjection.IHttpClientBuilder builder, string? appComponent=null);
```
#### Parameters

<a name='CIS.Infrastructure.ExternalServicesHelpers.HttpHandlersExtensions.AddExternalServicesKbHeaders(thisMicrosoft.Extensions.DependencyInjection.IHttpClientBuilder,string).builder'></a>

`builder` [Microsoft.Extensions.DependencyInjection.IHttpClientBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.DependencyInjection.IHttpClientBuilder 'Microsoft.Extensions.DependencyInjection.IHttpClientBuilder')

<a name='CIS.Infrastructure.ExternalServicesHelpers.HttpHandlersExtensions.AddExternalServicesKbHeaders(thisMicrosoft.Extensions.DependencyInjection.IHttpClientBuilder,string).appComponent'></a>

`appComponent` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Hodnota appComp v hlavičce X-KB-Caller-System-Identity. Pokud není vyplněno, je nastavena na "NOBY".

#### Returns
[Microsoft.Extensions.DependencyInjection.IHttpClientBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.DependencyInjection.IHttpClientBuilder 'Microsoft.Extensions.DependencyInjection.IHttpClientBuilder')
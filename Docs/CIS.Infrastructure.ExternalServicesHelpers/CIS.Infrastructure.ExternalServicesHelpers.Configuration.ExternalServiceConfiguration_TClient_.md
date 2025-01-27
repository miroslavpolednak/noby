#### [CIS.Infrastructure.ExternalServicesHelpers](index.md 'index')
### [CIS.Infrastructure.ExternalServicesHelpers.Configuration](CIS.Infrastructure.ExternalServicesHelpers.Configuration.md 'CIS.Infrastructure.ExternalServicesHelpers.Configuration')

## ExternalServiceConfiguration<TClient> Class

Výchozí implementace IExternalServiceConfiguration.

```csharp
public class ExternalServiceConfiguration<TClient> :
CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration<TClient>,
CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration,
CIS.Core.IIsServiceDiscoverable
    where TClient : class, CIS.Infrastructure.ExternalServicesHelpers.IExternalServiceClient
```
#### Type parameters

<a name='CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServiceConfiguration_TClient_.TClient'></a>

`TClient`

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; ExternalServiceConfiguration<TClient>

Implements [CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration&lt;](CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration_TClient_.md 'CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration<TClient>')[TClient](CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServiceConfiguration_TClient_.md#CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServiceConfiguration_TClient_.TClient 'CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServiceConfiguration<TClient>.TClient')[&gt;](CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration_TClient_.md 'CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration<TClient>'), [IExternalServiceConfiguration](CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.md 'CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration'), [CIS.Core.IIsServiceDiscoverable](https://docs.microsoft.com/en-us/dotnet/api/CIS.Core.IIsServiceDiscoverable 'CIS.Core.IIsServiceDiscoverable')
### Properties

<a name='CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServiceConfiguration_TClient_.Authentication'></a>

## ExternalServiceConfiguration<TClient>.Authentication Property

Typ pouzite autentizace na sluzbu treti strany

```csharp
public CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServicesAuthenticationTypes Authentication { get; set; }
```

Implements [Authentication](CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.md#CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.Authentication 'CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.Authentication')

#### Property Value
[ExternalServicesAuthenticationTypes](CIS.Infrastructure.ExternalServicesHelpers.Configuration.md#CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServicesAuthenticationTypes 'CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServicesAuthenticationTypes')

<a name='CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServiceConfiguration_TClient_.IgnoreServerCertificateErrors'></a>

## ExternalServiceConfiguration<TClient>.IgnoreServerCertificateErrors Property

Pokud =true, ignoruje HttpClient problem s SSL certifikatem remote serveru.

```csharp
public bool IgnoreServerCertificateErrors { get; set; }
```

Implements [IgnoreServerCertificateErrors](CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.md#CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.IgnoreServerCertificateErrors 'CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.IgnoreServerCertificateErrors')

#### Property Value
[System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')

<a name='CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServiceConfiguration_TClient_.ImplementationType'></a>

## ExternalServiceConfiguration<TClient>.ImplementationType Property

Type of http client implementation - can be mock or real client or something else.

```csharp
public CIS.Infrastructure.ExternalServicesHelpers.ServiceImplementationTypes ImplementationType { get; set; }
```

Implements [ImplementationType](CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.md#CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.ImplementationType 'CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.ImplementationType')

#### Property Value
[CIS.Infrastructure.ExternalServicesHelpers.ServiceImplementationTypes](https://docs.microsoft.com/en-us/dotnet/api/CIS.Infrastructure.ExternalServicesHelpers.ServiceImplementationTypes 'CIS.Infrastructure.ExternalServicesHelpers.ServiceImplementationTypes')

<a name='CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServiceConfiguration_TClient_.LogRequestPayload'></a>

## ExternalServiceConfiguration<TClient>.LogRequestPayload Property

True = do logu se ulozi plny payload odpovedi externi sluzby

```csharp
public bool LogRequestPayload { get; set; }
```

Implements [LogRequestPayload](CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.md#CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.LogRequestPayload 'CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.LogRequestPayload')

#### Property Value
[System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')

<a name='CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServiceConfiguration_TClient_.LogResponsePayload'></a>

## ExternalServiceConfiguration<TClient>.LogResponsePayload Property

True = do logu se ulozi plny request poslany do externi sluzby

```csharp
public bool LogResponsePayload { get; set; }
```

Implements [LogResponsePayload](CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.md#CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.LogResponsePayload 'CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.LogResponsePayload')

#### Property Value
[System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')

<a name='CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServiceConfiguration_TClient_.Password'></a>

## ExternalServiceConfiguration<TClient>.Password Property

Autentizace - Heslo

```csharp
public string? Password { get; set; }
```

Implements [Password](CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.md#CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.Password 'CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.Password')

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

<a name='CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServiceConfiguration_TClient_.RequestRetryCount'></a>

## ExternalServiceConfiguration<TClient>.RequestRetryCount Property

Pokud první request timeoutuje, zkus ještě X opakovat

```csharp
public System.Nullable<int> RequestRetryCount { get; set; }
```

Implements [RequestRetryCount](CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.md#CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.RequestRetryCount 'CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.RequestRetryCount')

#### Property Value
[System.Nullable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Nullable-1 'System.Nullable`1')[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Nullable-1 'System.Nullable`1')

<a name='CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServiceConfiguration_TClient_.RequestRetryTimeout'></a>

## ExternalServiceConfiguration<TClient>.RequestRetryTimeout Property

Mezi jednotlivými opakováními počkej X sekund

```csharp
public System.Nullable<int> RequestRetryTimeout { get; set; }
```

Implements [RequestRetryTimeout](CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.md#CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.RequestRetryTimeout 'CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.RequestRetryTimeout')

#### Property Value
[System.Nullable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Nullable-1 'System.Nullable`1')[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Nullable-1 'System.Nullable`1')

<a name='CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServiceConfiguration_TClient_.RequestTimeout'></a>

## ExternalServiceConfiguration<TClient>.RequestTimeout Property

Default single request timeout in seconds

```csharp
public System.Nullable<int> RequestTimeout { get; set; }
```

Implements [RequestTimeout](CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.md#CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.RequestTimeout 'CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.RequestTimeout')

#### Property Value
[System.Nullable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Nullable-1 'System.Nullable`1')[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Nullable-1 'System.Nullable`1')

### Remarks
Default is set to 10 seconds

<a name='CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServiceConfiguration_TClient_.ServiceName'></a>

## ExternalServiceConfiguration<TClient>.ServiceName Property

Nazev sluzby v ServiceDiscovery

```csharp
public string? ServiceName { get; set; }
```

Implements [ServiceName](https://docs.microsoft.com/en-us/dotnet/api/CIS.Core.IIsServiceDiscoverable.ServiceName 'CIS.Core.IIsServiceDiscoverable.ServiceName')

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

<a name='CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServiceConfiguration_TClient_.ServiceType'></a>

## ExternalServiceConfiguration<TClient>.ServiceType Property

Pro sluzby tretich stran vzdy 3

```csharp
public int ServiceType { get; }
```

Implements [ServiceType](https://docs.microsoft.com/en-us/dotnet/api/CIS.Core.IIsServiceDiscoverable.ServiceType 'CIS.Core.IIsServiceDiscoverable.ServiceType')

#### Property Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServiceConfiguration_TClient_.ServiceUrl'></a>

## ExternalServiceConfiguration<TClient>.ServiceUrl Property

Service URL when ServiceDiscovery is not being used. Use only when UseServiceDiscovery=false.

```csharp
public System.Uri? ServiceUrl { get; set; }
```

Implements [ServiceUrl](https://docs.microsoft.com/en-us/dotnet/api/CIS.Core.IIsServiceDiscoverable.ServiceUrl 'CIS.Core.IIsServiceDiscoverable.ServiceUrl')

#### Property Value
[System.Uri](https://docs.microsoft.com/en-us/dotnet/api/System.Uri 'System.Uri')

<a name='CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServiceConfiguration_TClient_.UseDefaultProxy'></a>

## ExternalServiceConfiguration<TClient>.UseDefaultProxy Property

Pokud je true, pouzije pro HttpClient systemovou proxy

```csharp
public bool UseDefaultProxy { get; set; }
```

Implements [UseDefaultProxy](CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.md#CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.UseDefaultProxy 'CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.UseDefaultProxy')

#### Property Value
[System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')

<a name='CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServiceConfiguration_TClient_.UseLogging'></a>

## ExternalServiceConfiguration<TClient>.UseLogging Property

Zapne logovani request a response payloadu a hlavicek. Default: true

```csharp
public bool UseLogging { get; set; }
```

Implements [UseLogging](CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.md#CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.UseLogging 'CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.UseLogging')

#### Property Value
[System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')

### Remarks
Je v konfiguraci, aby bylo možné měnit nastavení na úrovni CI/CD.

<a name='CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServiceConfiguration_TClient_.Username'></a>

## ExternalServiceConfiguration<TClient>.Username Property

Autentizace - Username

```csharp
public string? Username { get; set; }
```

Implements [Username](CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.md#CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.Username 'CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.Username')

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

<a name='CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServiceConfiguration_TClient_.UseServiceDiscovery'></a>

## ExternalServiceConfiguration<TClient>.UseServiceDiscovery Property

If True, then library will try to obtain all needed service URL's from ServiceDiscovery.

```csharp
public bool UseServiceDiscovery { get; set; }
```

Implements [UseServiceDiscovery](https://docs.microsoft.com/en-us/dotnet/api/CIS.Core.IIsServiceDiscoverable.UseServiceDiscovery 'CIS.Core.IIsServiceDiscoverable.UseServiceDiscovery')

#### Property Value
[System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')

### Remarks
Default is set to True
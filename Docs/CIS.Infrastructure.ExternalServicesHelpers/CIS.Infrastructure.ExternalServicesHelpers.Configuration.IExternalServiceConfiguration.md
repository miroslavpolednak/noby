#### [CIS.Infrastructure.ExternalServicesHelpers](index.md 'index')
### [CIS.Infrastructure.ExternalServicesHelpers.Configuration](CIS.Infrastructure.ExternalServicesHelpers.Configuration.md 'CIS.Infrastructure.ExternalServicesHelpers.Configuration')

## IExternalServiceConfiguration Interface

Základní konfigurace externí služby (služby třetí strany).

```csharp
public interface IExternalServiceConfiguration :
CIS.Core.IIsServiceDiscoverable
```

Derived  
&#8627; [ExternalServiceConfiguration&lt;TClient&gt;](CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServiceConfiguration_TClient_.md 'CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServiceConfiguration<TClient>')  
&#8627; [IExternalServiceConfiguration&lt;TClient&gt;](CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration_TClient_.md 'CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration<TClient>')

Implements [CIS.Core.IIsServiceDiscoverable](https://docs.microsoft.com/en-us/dotnet/api/CIS.Core.IIsServiceDiscoverable 'CIS.Core.IIsServiceDiscoverable')

### Remarks
Pro registraci HTTP klienta by se vždy měla používat generická verze interface.
### Properties

<a name='CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.Authentication'></a>

## IExternalServiceConfiguration.Authentication Property

Typ pouzite autentizace na sluzbu treti strany

```csharp
CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServicesAuthenticationTypes Authentication { get; set; }
```

#### Property Value
[ExternalServicesAuthenticationTypes](CIS.Infrastructure.ExternalServicesHelpers.Configuration.md#CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServicesAuthenticationTypes 'CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServicesAuthenticationTypes')

<a name='CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.IgnoreServerCertificateErrors'></a>

## IExternalServiceConfiguration.IgnoreServerCertificateErrors Property

Pokud =true, ignoruje HttpClient problem s SSL certifikatem remote serveru.

```csharp
bool IgnoreServerCertificateErrors { get; set; }
```

#### Property Value
[System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')

<a name='CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.ImplementationType'></a>

## IExternalServiceConfiguration.ImplementationType Property

Type of http client implementation - can be mock or real client or something else.

```csharp
CIS.Foms.Enums.ServiceImplementationTypes ImplementationType { get; set; }
```

#### Property Value
[CIS.Foms.Enums.ServiceImplementationTypes](https://docs.microsoft.com/en-us/dotnet/api/CIS.Foms.Enums.ServiceImplementationTypes 'CIS.Foms.Enums.ServiceImplementationTypes')

<a name='CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.LogPayloads'></a>

## IExternalServiceConfiguration.LogPayloads Property

Zapne logovani request a response payloadu a hlavicek. Default: true

```csharp
bool LogPayloads { get; set; }
```

#### Property Value
[System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')

### Remarks
Je v konfiguraci, aby bylo možné měnit nastavení na úrovni CI/CD.

<a name='CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.Password'></a>

## IExternalServiceConfiguration.Password Property

Autentizace - Heslo

```csharp
string? Password { get; set; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

<a name='CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.RequestTimeout'></a>

## IExternalServiceConfiguration.RequestTimeout Property

Default request timeout in seconds

```csharp
System.Nullable<int> RequestTimeout { get; set; }
```

#### Property Value
[System.Nullable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Nullable-1 'System.Nullable`1')[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Nullable-1 'System.Nullable`1')

### Remarks
Default is set to 10 seconds

<a name='CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.Username'></a>

## IExternalServiceConfiguration.Username Property

Autentizace - Username

```csharp
string? Username { get; set; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')
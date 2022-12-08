#### [CIS.Infrastructure.ExternalServicesHelpers](index.md 'index')
### [CIS.Infrastructure.ExternalServicesHelpers.Configuration](CIS.Infrastructure.ExternalServicesHelpers.Configuration.md 'CIS.Infrastructure.ExternalServicesHelpers.Configuration')

## ExternalServiceBasicAuthenticationConfiguration<TClient> Class

Výchozí implementace IExternalServiceBasicAuthenticationConfiguration

```csharp
public class ExternalServiceBasicAuthenticationConfiguration<TClient> : CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServiceConfiguration<TClient>,
CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceBasicAuthenticationConfiguration
    where TClient : class, CIS.Infrastructure.ExternalServicesHelpers.IExternalServiceClient
```
#### Type parameters

<a name='CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServiceBasicAuthenticationConfiguration_TClient_.TClient'></a>

`TClient`

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; [CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServiceConfiguration&lt;](CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServiceConfiguration_TClient_.md 'CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServiceConfiguration<TClient>')[TClient](CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServiceBasicAuthenticationConfiguration_TClient_.md#CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServiceBasicAuthenticationConfiguration_TClient_.TClient 'CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServiceBasicAuthenticationConfiguration<TClient>.TClient')[&gt;](CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServiceConfiguration_TClient_.md 'CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServiceConfiguration<TClient>') &#129106; ExternalServiceBasicAuthenticationConfiguration<TClient>

Implements [IExternalServiceBasicAuthenticationConfiguration](CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceBasicAuthenticationConfiguration.md 'CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceBasicAuthenticationConfiguration')
### Properties

<a name='CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServiceBasicAuthenticationConfiguration_TClient_.Password'></a>

## ExternalServiceBasicAuthenticationConfiguration<TClient>.Password Property

Heslo

```csharp
public string? Password { get; set; }
```

Implements [Password](CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceBasicAuthenticationConfiguration.md#CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceBasicAuthenticationConfiguration.Password 'CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceBasicAuthenticationConfiguration.Password')

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

<a name='CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServiceBasicAuthenticationConfiguration_TClient_.Username'></a>

## ExternalServiceBasicAuthenticationConfiguration<TClient>.Username Property

Username

```csharp
public string? Username { get; set; }
```

Implements [Username](CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceBasicAuthenticationConfiguration.md#CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceBasicAuthenticationConfiguration.Username 'CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceBasicAuthenticationConfiguration.Username')

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')
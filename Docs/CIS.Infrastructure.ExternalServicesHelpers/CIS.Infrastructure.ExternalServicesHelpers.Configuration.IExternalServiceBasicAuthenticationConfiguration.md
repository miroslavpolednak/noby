#### [CIS.Infrastructure.ExternalServicesHelpers](index.md 'index')
### [CIS.Infrastructure.ExternalServicesHelpers.Configuration](CIS.Infrastructure.ExternalServicesHelpers.Configuration.md 'CIS.Infrastructure.ExternalServicesHelpers.Configuration')

## IExternalServiceBasicAuthenticationConfiguration Interface

Konfigurace pro podporu HTTP Basic Authentication.

```csharp
public interface IExternalServiceBasicAuthenticationConfiguration
```

Derived  
&#8627; [ExternalServiceBasicAuthenticationConfiguration&lt;TClient&gt;](CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServiceBasicAuthenticationConfiguration_TClient_.md 'CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServiceBasicAuthenticationConfiguration<TClient>')
### Properties

<a name='CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceBasicAuthenticationConfiguration.Password'></a>

## IExternalServiceBasicAuthenticationConfiguration.Password Property

Heslo

```csharp
string? Password { get; set; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

<a name='CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceBasicAuthenticationConfiguration.Username'></a>

## IExternalServiceBasicAuthenticationConfiguration.Username Property

Username

```csharp
string? Username { get; set; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')
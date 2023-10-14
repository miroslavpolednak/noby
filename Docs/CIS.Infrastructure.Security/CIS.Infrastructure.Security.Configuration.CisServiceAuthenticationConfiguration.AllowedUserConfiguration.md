#### [CIS.Infrastructure.Security](index.md 'index')
### [CIS.Infrastructure.Security.Configuration](CIS.Infrastructure.Security.Configuration.md 'CIS.Infrastructure.Security.Configuration').[CisServiceAuthenticationConfiguration](CIS.Infrastructure.Security.Configuration.CisServiceAuthenticationConfiguration.md 'CIS.Infrastructure.Security.Configuration.CisServiceAuthenticationConfiguration')

## CisServiceAuthenticationConfiguration.AllowedUserConfiguration Class

```csharp
public sealed class CisServiceAuthenticationConfiguration.AllowedUserConfiguration
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; AllowedUserConfiguration
### Properties

<a name='CIS.Infrastructure.Security.Configuration.CisServiceAuthenticationConfiguration.AllowedUserConfiguration.Roles'></a>

## CisServiceAuthenticationConfiguration.AllowedUserConfiguration.Roles Property

Seznam rolí ve kterých je uživatel zařazen - nemusí mít žádné

```csharp
public System.Collections.Generic.List<string>? Roles { get; set; }
```

#### Property Value
[System.Collections.Generic.List&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.List-1 'System.Collections.Generic.List`1')[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.List-1 'System.Collections.Generic.List`1')

<a name='CIS.Infrastructure.Security.Configuration.CisServiceAuthenticationConfiguration.AllowedUserConfiguration.Username'></a>

## CisServiceAuthenticationConfiguration.AllowedUserConfiguration.Username Property

Login technického uživatele

```csharp
public string Username { get; set; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')
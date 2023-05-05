#### [CIS.Infrastructure.Security](index.md 'index')
### [CIS.Infrastructure.Security.Configuration](CIS.Infrastructure.Security.Configuration.md 'CIS.Infrastructure.Security.Configuration')

## CisServiceAuthenticationConfiguration Class

Konfigurace autentizace doménové služby

```csharp
public sealed class CisServiceAuthenticationConfiguration
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; CisServiceAuthenticationConfiguration
### Properties

<a name='CIS.Infrastructure.Security.Configuration.CisServiceAuthenticationConfiguration.AdHost'></a>

## CisServiceAuthenticationConfiguration.AdHost Property

Adresa domenoveho serveru

```csharp
public string? AdHost { get; set; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

<a name='CIS.Infrastructure.Security.Configuration.CisServiceAuthenticationConfiguration.AdPort'></a>

## CisServiceAuthenticationConfiguration.AdPort Property

Port na domenovem serveru, vychozi je 389

```csharp
public System.Nullable<int> AdPort { get; set; }
```

#### Property Value
[System.Nullable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Nullable-1 'System.Nullable`1')[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Nullable-1 'System.Nullable`1')

<a name='CIS.Infrastructure.Security.Configuration.CisServiceAuthenticationConfiguration.DomainUsernamePrefix'></a>

## CisServiceAuthenticationConfiguration.DomainUsernamePrefix Property

Domena ve ktere je umisten autentizovany uzivatel. Napr. "vsskb\"  
Pozor, musi byt vcetne \ na konci

```csharp
public string? DomainUsernamePrefix { get; set; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')
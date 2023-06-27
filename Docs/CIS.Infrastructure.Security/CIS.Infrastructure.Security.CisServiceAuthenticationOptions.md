#### [CIS.Infrastructure.Security](index.md 'index')
### [CIS.Infrastructure.Security](CIS.Infrastructure.Security.md 'CIS.Infrastructure.Security')

## CisServiceAuthenticationOptions Class

```csharp
public sealed class CisServiceAuthenticationOptions : Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; [Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions 'Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions') &#129106; CisServiceAuthenticationOptions
### Properties

<a name='CIS.Infrastructure.Security.CisServiceAuthenticationOptions.AdHost'></a>

## CisServiceAuthenticationOptions.AdHost Property

Adresa domenoveho serveru

```csharp
public string? AdHost { get; set; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

<a name='CIS.Infrastructure.Security.CisServiceAuthenticationOptions.AdPort'></a>

## CisServiceAuthenticationOptions.AdPort Property

Port na domenovem serveru, vychozi je 389

```csharp
public int AdPort { get; set; }
```

#### Property Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='CIS.Infrastructure.Security.CisServiceAuthenticationOptions.Domain'></a>

## CisServiceAuthenticationOptions.Domain Property

Domena ve ktere je umisten autentizovany uzivatel. Napr. "vsskb\"  
Pozor, musi byt vcetne \ na konci

```csharp
public string? Domain { get; set; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

<a name='CIS.Infrastructure.Security.CisServiceAuthenticationOptions.IsSsl'></a>

## CisServiceAuthenticationOptions.IsSsl Property

True pokud se jedna o SSL connection

```csharp
public bool IsSsl { get; set; }
```

#### Property Value
[System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')
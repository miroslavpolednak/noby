#### [CIS.Core](index.md 'index')
### [CIS.Core.Exceptions](CIS.Core.Exceptions.md 'CIS.Core.Exceptions')

## CisAuthenticationException Class

Chyba, která označuje problém s validací koncového uživatele.

```csharp
public sealed class CisAuthenticationException : System.Security.Authentication.AuthenticationException
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; [System.Exception](https://docs.microsoft.com/en-us/dotnet/api/System.Exception 'System.Exception') &#129106; [System.SystemException](https://docs.microsoft.com/en-us/dotnet/api/System.SystemException 'System.SystemException') &#129106; [System.Security.Authentication.AuthenticationException](https://docs.microsoft.com/en-us/dotnet/api/System.Security.Authentication.AuthenticationException 'System.Security.Authentication.AuthenticationException') &#129106; CisAuthenticationException
### Properties

<a name='CIS.Core.Exceptions.CisAuthenticationException.ProviderLoginUrl'></a>

## CisAuthenticationException.ProviderLoginUrl Property

Adresa providera autentizace, na kterou má být uživatel přesměrován pro přihlášení.

```csharp
public string? ProviderLoginUrl { get; set; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

### Remarks
Je použité v případě, že se jedná o autentizaci frontendu, kdy chceme FE vrátit informaci o tom, kam má uživatele přesměrovat.
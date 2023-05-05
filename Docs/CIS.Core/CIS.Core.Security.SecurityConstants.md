#### [CIS.Core](index.md 'index')
### [CIS.Core.Security](CIS.Core.Security.md 'CIS.Core.Security')

## SecurityConstants Class

Konstanty pro nastavení auth providerů atd.

```csharp
public sealed class SecurityConstants
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; SecurityConstants
### Fields

<a name='CIS.Core.Security.SecurityConstants.ClaimTypeIdent'></a>

## SecurityConstants.ClaimTypeIdent Field

Type claimu, který obsahuje login (CAAS login) přihlášeného uživatele

```csharp
public const string ClaimTypeIdent = noby-user-ident;
```

#### Field Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

<a name='CIS.Core.Security.SecurityConstants.ContextUserHttpHeaderUserIdentKey'></a>

## SecurityConstants.ContextUserHttpHeaderUserIdentKey Field

Klíč pro HTTP hlavičku s informací o loginu (CAAS) aktuálně přihlášeného uživatele.

```csharp
public const string ContextUserHttpHeaderUserIdentKey = noby-user-ident;
```

#### Field Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

<a name='CIS.Core.Security.SecurityConstants.ContextUserHttpHeaderUserIdKey'></a>

## SecurityConstants.ContextUserHttpHeaderUserIdKey Field

Klíč pro HTTP hlavičku s informací o v33id aktuálně přihlášeného uživatele.

```csharp
public const string ContextUserHttpHeaderUserIdKey = noby-user-id;
```

#### Field Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')
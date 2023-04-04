#### [CIS.Core](index.md 'index')
### [CIS.Core.Security](CIS.Core.Security.md 'CIS.Core.Security')

## ICurrentUser Interface

Instance aktuálně přihlášeného fyzického uživatele

```csharp
public interface ICurrentUser
```
### Properties

<a name='CIS.Core.Security.ICurrentUser.DisplayName'></a>

## ICurrentUser.DisplayName Property

Jmeno a prijmeni uzivatele

```csharp
string? DisplayName { get; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

<a name='CIS.Core.Security.ICurrentUser.Id'></a>

## ICurrentUser.Id Property

v33id

```csharp
int Id { get; }
```

#### Property Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='CIS.Core.Security.ICurrentUser.Login'></a>

## ICurrentUser.Login Property

Login uzivatele

```csharp
string? Login { get; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')
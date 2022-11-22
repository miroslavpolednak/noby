#### [CIS.Core](index.md 'index')
### [CIS.Core.Security](CIS.Core.Security.md 'CIS.Core.Security')

## IServiceUserAccessor Interface

Helper pro ziskani instance technickeho uzivatele, pod kterym je spusten request na interni sluzbu.

```csharp
public interface IServiceUserAccessor
```
### Properties

<a name='CIS.Core.Security.IServiceUserAccessor.IsAuthenticated'></a>

## IServiceUserAccessor.IsAuthenticated Property

Pokud je false, uzivatel neni autentikovan - User = null

```csharp
bool IsAuthenticated { get; }
```

#### Property Value
[System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')

<a name='CIS.Core.Security.IServiceUserAccessor.User'></a>

## IServiceUserAccessor.User Property

Instance technickeho uzivatele

```csharp
CIS.Core.Security.IServiceUser? User { get; }
```

#### Property Value
[IServiceUser](CIS.Core.Security.IServiceUser.md 'CIS.Core.Security.IServiceUser')
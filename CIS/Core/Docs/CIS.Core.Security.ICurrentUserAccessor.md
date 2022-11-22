#### [CIS.Core](index.md 'index')
### [CIS.Core.Security](CIS.Core.Security.md 'CIS.Core.Security')

## ICurrentUserAccessor Interface

Helper pro ziskani fyzickeho uzivatele, ktery aplikaci/sluzbu vola

```csharp
public interface ICurrentUserAccessor
```
### Properties

<a name='CIS.Core.Security.ICurrentUserAccessor.IsAuthenticated'></a>

## ICurrentUserAccessor.IsAuthenticated Property

Pokud je false, uzivatel neni autentikovan - User = null

```csharp
bool IsAuthenticated { get; }
```

#### Property Value
[System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')

<a name='CIS.Core.Security.ICurrentUserAccessor.User'></a>

## ICurrentUserAccessor.User Property

Zakladni data o uzivateli - Id, login?

```csharp
CIS.Core.Security.ICurrentUser? User { get; }
```

#### Property Value
[ICurrentUser](CIS.Core.Security.ICurrentUser.md 'CIS.Core.Security.ICurrentUser')

<a name='CIS.Core.Security.ICurrentUserAccessor.UserDetails'></a>

## ICurrentUserAccessor.UserDetails Property

Kompletni profil uzivatele - neni implicitne naplnen. Pro jeho naplneni je potreba zavolat FetchDetails().

```csharp
CIS.Core.Security.ICurrentUserDetails? UserDetails { get; }
```

#### Property Value
[CIS.Core.Security.ICurrentUserDetails](https://docs.microsoft.com/en-us/dotnet/api/CIS.Core.Security.ICurrentUserDetails 'CIS.Core.Security.ICurrentUserDetails')
### Methods

<a name='CIS.Core.Security.ICurrentUserAccessor.EnsureDetails(System.Threading.CancellationToken)'></a>

## ICurrentUserAccessor.EnsureDetails(CancellationToken) Method

Pokud se tak uz nestalo, naplni profil uzivatele daty z UserService

```csharp
System.Threading.Tasks.Task<CIS.Core.Security.ICurrentUserDetails> EnsureDetails(System.Threading.CancellationToken cancellationToken);
```
#### Parameters

<a name='CIS.Core.Security.ICurrentUserAccessor.EnsureDetails(System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System.Threading.CancellationToken](https://docs.microsoft.com/en-us/dotnet/api/System.Threading.CancellationToken 'System.Threading.CancellationToken')

#### Returns
[System.Threading.Tasks.Task&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Threading.Tasks.Task-1 'System.Threading.Tasks.Task`1')[CIS.Core.Security.ICurrentUserDetails](https://docs.microsoft.com/en-us/dotnet/api/CIS.Core.Security.ICurrentUserDetails 'CIS.Core.Security.ICurrentUserDetails')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Threading.Tasks.Task-1 'System.Threading.Tasks.Task`1')
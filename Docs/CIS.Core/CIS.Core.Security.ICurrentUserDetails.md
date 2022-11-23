#### [CIS.Core](index.md 'index')
### [CIS.Core.Security](CIS.Core.Security.md 'CIS.Core.Security')

## ICurrentUserDetails Interface

Další informace o uživateli.

```csharp
public interface ICurrentUserDetails
```

### Remarks
Tyto informace nejsou v [ICurrentUser](CIS.Core.Security.ICurrentUser.md 'CIS.Core.Security.ICurrentUser'), protože se mohou systém od systému lišit, ale ICurrentUser je stejný pro všechny a nelze ho upravovat per systém.
### Properties

<a name='CIS.Core.Security.ICurrentUserDetails.DisplayName'></a>

## ICurrentUserDetails.DisplayName Property

Celé jméno uživatele

```csharp
string DisplayName { get; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

### Remarks
Filip Tůma
#### [CIS.Core](index.md 'index')
### [CIS.Core.Data](CIS.Core.Data.md 'CIS.Core.Data')

## IModifiedUser Interface

EF entita obsahuje sloupce s informací o uživateli, který ji naposledy updatoval.

```csharp
public interface IModifiedUser
```

Derived  
&#8627; [BaseCreatedWithModifiedUserId](CIS.Core.Data.BaseCreatedWithModifiedUserId.md 'CIS.Core.Data.BaseCreatedWithModifiedUserId')  
&#8627; [BaseModifiedUser](CIS.Core.Data.BaseModifiedUser.md 'CIS.Core.Data.BaseModifiedUser')
### Properties

<a name='CIS.Core.Data.IModifiedUser.ModifiedUserId'></a>

## IModifiedUser.ModifiedUserId Property

v33id uživatele

```csharp
System.Nullable<int> ModifiedUserId { get; set; }
```

#### Property Value
[System.Nullable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Nullable-1 'System.Nullable`1')[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Nullable-1 'System.Nullable`1')

<a name='CIS.Core.Data.IModifiedUser.ModifiedUserName'></a>

## IModifiedUser.ModifiedUserName Property

Jméno a příjmení uživatele

```csharp
string? ModifiedUserName { get; set; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')
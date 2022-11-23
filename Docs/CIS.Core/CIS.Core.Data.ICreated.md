#### [CIS.Core](index.md 'index')
### [CIS.Core.Data](CIS.Core.Data.md 'CIS.Core.Data')

## ICreated Interface

EF entita obsahuje sloupce s informací o uživateli, který ji vytvořil.

```csharp
public interface ICreated
```

Derived  
&#8627; [BaseCreated](CIS.Core.Data.BaseCreated.md 'CIS.Core.Data.BaseCreated')
### Properties

<a name='CIS.Core.Data.ICreated.CreatedTime'></a>

## ICreated.CreatedTime Property

Datum vytvoření entity

```csharp
System.DateTime CreatedTime { get; set; }
```

#### Property Value
[System.DateTime](https://docs.microsoft.com/en-us/dotnet/api/System.DateTime 'System.DateTime')

<a name='CIS.Core.Data.ICreated.CreatedUserId'></a>

## ICreated.CreatedUserId Property

v33id uživatele

```csharp
System.Nullable<int> CreatedUserId { get; set; }
```

#### Property Value
[System.Nullable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Nullable-1 'System.Nullable`1')[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Nullable-1 'System.Nullable`1')

<a name='CIS.Core.Data.ICreated.CreatedUserName'></a>

## ICreated.CreatedUserName Property

Jméno a příjmení uživatele

```csharp
string? CreatedUserName { get; set; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')
#### [CIS.Core](index.md 'index')
### [CIS.Core.Types](CIS.Core.Types.md 'CIS.Core.Types')

## IPaginableRequest Interface

Obecný request model pro Mediator request podporující stránkování.

```csharp
public interface IPaginableRequest
```

Derived  
&#8627; [Paginable](CIS.Core.Types.Paginable.md 'CIS.Core.Types.Paginable')

### Remarks
Používá se pro gRPC i Webapi aplikace.
### Properties

<a name='CIS.Core.Types.IPaginableRequest.HasSorting'></a>

## IPaginableRequest.HasSorting Property

Informace o tom, zda aktuální instance requestu obsahuje nastavení řazení záznamů.

```csharp
bool HasSorting { get; }
```

#### Property Value
[System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')

<a name='CIS.Core.Types.IPaginableRequest.PageSize'></a>

## IPaginableRequest.PageSize Property

Velikost jedné stránky - počet záznamů.

```csharp
int PageSize { get; }
```

#### Property Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

### Example
10

<a name='CIS.Core.Types.IPaginableRequest.RecordOffset'></a>

## IPaginableRequest.RecordOffset Property

Offset prvního záznamu vytaženého ze zdrojových dat - zero based.

```csharp
int RecordOffset { get; }
```

#### Property Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

### Example
0
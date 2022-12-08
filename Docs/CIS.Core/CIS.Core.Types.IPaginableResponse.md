#### [CIS.Core](index.md 'index')
### [CIS.Core.Types](CIS.Core.Types.md 'CIS.Core.Types')

## IPaginableResponse Interface

Obecný resopnse model pro Mediator request podporující stránkování.

```csharp
public interface IPaginableResponse
```
### Properties

<a name='CIS.Core.Types.IPaginableResponse.PageSize'></a>

## IPaginableResponse.PageSize Property

Aktuální velikost stránky - počet záznamů v tomto response.

```csharp
int PageSize { get; }
```

#### Property Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

### Example
10

<a name='CIS.Core.Types.IPaginableResponse.RecordOffset'></a>

## IPaginableResponse.RecordOffset Property

Offset prvního záznamu vytaženého ze zdrojových dat - zero based.

```csharp
int RecordOffset { get; }
```

#### Property Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

### Example
0

<a name='CIS.Core.Types.IPaginableResponse.RecordsTotalSize'></a>

## IPaginableResponse.RecordsTotalSize Property

Celkový počet záznamů nalezených bez ohledu na stránkování.

```csharp
int RecordsTotalSize { get; }
```

#### Property Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')
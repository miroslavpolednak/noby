#### [CIS.Infrastructure.WebApi](index.md 'index')
### [CIS.Infrastructure.WebApi.Types](CIS.Infrastructure.WebApi.Types.md 'CIS.Infrastructure.WebApi.Types')

## PaginationRequest Class

```csharp
public class PaginationRequest :
CIS.Core.Types.IPaginableRequest
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; PaginationRequest

Implements [CIS.Core.Types.IPaginableRequest](https://docs.microsoft.com/en-us/dotnet/api/CIS.Core.Types.IPaginableRequest 'CIS.Core.Types.IPaginableRequest')
### Properties

<a name='CIS.Infrastructure.WebApi.Types.PaginationRequest.PageSize'></a>

## PaginationRequest.PageSize Property

Pocet zaznamu na jedne strance

```csharp
public int PageSize { get; set; }
```

Implements [PageSize](https://docs.microsoft.com/en-us/dotnet/api/CIS.Core.Types.IPaginableRequest.PageSize 'CIS.Core.Types.IPaginableRequest.PageSize')

#### Property Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

### Example
10

<a name='CIS.Infrastructure.WebApi.Types.PaginationRequest.RecordOffset'></a>

## PaginationRequest.RecordOffset Property

Offset (index, start=0) zaznamu, od ktereho se ma zacit s nacitanim

```csharp
public int RecordOffset { get; set; }
```

Implements [RecordOffset](https://docs.microsoft.com/en-us/dotnet/api/CIS.Core.Types.IPaginableRequest.RecordOffset 'CIS.Core.Types.IPaginableRequest.RecordOffset')

#### Property Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

### Example
0

<a name='CIS.Infrastructure.WebApi.Types.PaginationRequest.Sorting'></a>

## PaginationRequest.Sorting Property

[optional] Nastaveni razeni

```csharp
public System.Collections.Generic.List<CIS.Infrastructure.WebApi.Types.PaginationSortingField>? Sorting { get; set; }
```

#### Property Value
[System.Collections.Generic.List&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.List-1 'System.Collections.Generic.List`1')[CIS.Infrastructure.WebApi.Types.PaginationSortingField](https://docs.microsoft.com/en-us/dotnet/api/CIS.Infrastructure.WebApi.Types.PaginationSortingField 'CIS.Infrastructure.WebApi.Types.PaginationSortingField')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.List-1 'System.Collections.Generic.List`1')

<a name='CIS.Infrastructure.WebApi.Types.PaginationRequest.TypeOfSortingField'></a>

## PaginationRequest.TypeOfSortingField Property

Pro interoperabilitu s gRPC typem PaginationRequest

```csharp
public System.Type TypeOfSortingField { get; }
```

Implements [TypeOfSortingField](https://docs.microsoft.com/en-us/dotnet/api/CIS.Core.Types.IPaginableRequest.TypeOfSortingField 'CIS.Core.Types.IPaginableRequest.TypeOfSortingField')

#### Property Value
[System.Type](https://docs.microsoft.com/en-us/dotnet/api/System.Type 'System.Type')
### Methods

<a name='CIS.Infrastructure.WebApi.Types.PaginationRequest.GetSorting()'></a>

## PaginationRequest.GetSorting() Method

Pro interoperabilitu s gRPC typem PaginationRequest

```csharp
public System.Collections.Generic.IEnumerable<CIS.Core.Types.IPaginableSortingField>? GetSorting();
```

Implements [GetSorting()](https://docs.microsoft.com/en-us/dotnet/api/CIS.Core.Types.IPaginableRequest.GetSorting 'CIS.Core.Types.IPaginableRequest.GetSorting')

#### Returns
[System.Collections.Generic.IEnumerable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.IEnumerable-1 'System.Collections.Generic.IEnumerable`1')[CIS.Core.Types.IPaginableSortingField](https://docs.microsoft.com/en-us/dotnet/api/CIS.Core.Types.IPaginableSortingField 'CIS.Core.Types.IPaginableSortingField')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.IEnumerable-1 'System.Collections.Generic.IEnumerable`1')
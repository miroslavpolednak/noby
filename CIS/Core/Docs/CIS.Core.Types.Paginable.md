#### [CIS.Core](index.md 'index')
### [CIS.Core.Types](CIS.Core.Types.md 'CIS.Core.Types')

## Paginable Class

Implementace [IPaginableRequest](CIS.Core.Types.IPaginableRequest.md 'CIS.Core.Types.IPaginableRequest')

```csharp
public sealed class Paginable :
CIS.Core.Types.IPaginableRequest
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; Paginable

Implements [IPaginableRequest](CIS.Core.Types.IPaginableRequest.md 'CIS.Core.Types.IPaginableRequest')
### Methods

<a name='CIS.Core.Types.Paginable.EnsureAndTranslateSortFields(System.Collections.Generic.IEnumerable_CIS.Core.Types.Paginable.MapperField_)'></a>

## Paginable.EnsureAndTranslateSortFields(IEnumerable<MapperField>) Method

Premapuje Field names na hodnoty v databazi. Zaroven kontroluje, ze v mapperu jsou vsechna Field names z puvodni kolekce.

```csharp
public CIS.Core.Types.Paginable EnsureAndTranslateSortFields(System.Collections.Generic.IEnumerable<CIS.Core.Types.Paginable.MapperField> mapper);
```
#### Parameters

<a name='CIS.Core.Types.Paginable.EnsureAndTranslateSortFields(System.Collections.Generic.IEnumerable_CIS.Core.Types.Paginable.MapperField_).mapper'></a>

`mapper` [System.Collections.Generic.IEnumerable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.IEnumerable-1 'System.Collections.Generic.IEnumerable`1')[CIS.Core.Types.Paginable.MapperField](https://docs.microsoft.com/en-us/dotnet/api/CIS.Core.Types.Paginable.MapperField 'CIS.Core.Types.Paginable.MapperField')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.IEnumerable-1 'System.Collections.Generic.IEnumerable`1')

#### Returns
[Paginable](CIS.Core.Types.Paginable.md 'CIS.Core.Types.Paginable')
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

#### Exceptions

[CisValidationException](CIS.Core.Exceptions.CisValidationException.md 'CIS.Core.Exceptions.CisValidationException')  
Sort Field not allowed

<a name='CIS.Core.Types.Paginable.FromRequest(CIS.Core.Types.IPaginableRequest,int,int)'></a>

## Paginable.FromRequest(IPaginableRequest, int, int) Method

Vytvori request s podporou strankovani z jiného IPaginableRequest.

```csharp
public static CIS.Core.Types.Paginable FromRequest(CIS.Core.Types.IPaginableRequest? request, int defaultPageSize=10, int defaultRecordOffset=0);
```
#### Parameters

<a name='CIS.Core.Types.Paginable.FromRequest(CIS.Core.Types.IPaginableRequest,int,int).request'></a>

`request` [IPaginableRequest](CIS.Core.Types.IPaginableRequest.md 'CIS.Core.Types.IPaginableRequest')

<a name='CIS.Core.Types.Paginable.FromRequest(CIS.Core.Types.IPaginableRequest,int,int).defaultPageSize'></a>

`defaultPageSize` [System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='CIS.Core.Types.Paginable.FromRequest(CIS.Core.Types.IPaginableRequest,int,int).defaultRecordOffset'></a>

`defaultRecordOffset` [System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

#### Returns
[Paginable](CIS.Core.Types.Paginable.md 'CIS.Core.Types.Paginable')

### Remarks
Používá se pro konverzi s FE requestu na gRPC request, což jsou dvě různé implementace.

<a name='CIS.Core.Types.Paginable.SetDefaultSort(string,bool)'></a>

## Paginable.SetDefaultSort(string, bool) Method

Nastaví requestu výchozí řazení - pouze pokud již není řazení nastaveno.

```csharp
public CIS.Core.Types.Paginable SetDefaultSort(string field, bool descending);
```
#### Parameters

<a name='CIS.Core.Types.Paginable.SetDefaultSort(string,bool).field'></a>

`field` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

<a name='CIS.Core.Types.Paginable.SetDefaultSort(string,bool).descending'></a>

`descending` [System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')

#### Returns
[Paginable](CIS.Core.Types.Paginable.md 'CIS.Core.Types.Paginable')
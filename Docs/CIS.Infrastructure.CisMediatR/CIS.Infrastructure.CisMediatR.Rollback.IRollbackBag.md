#### [CIS.Infrastructure.CisMediatR](index.md 'index')
### [CIS.Infrastructure.CisMediatR.Rollback](CIS.Infrastructure.CisMediatR.Rollback.md 'CIS.Infrastructure.CisMediatR.Rollback')

## IRollbackBag Interface

Uloziste umoznujici prenos vybranych dat z Meditr handleru do rollback handleru.

```csharp
public interface IRollbackBag
```
### Properties

<a name='CIS.Infrastructure.CisMediatR.Rollback.IRollbackBag.Count'></a>

## IRollbackBag.Count Property

Pocet polozek vlozenych do bagu

```csharp
int Count { get; }
```

#### Property Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')
### Methods

<a name='CIS.Infrastructure.CisMediatR.Rollback.IRollbackBag.Add(string,object)'></a>

## IRollbackBag.Add(string, object) Method

Prida dalsi polozku do bagu. Jednotlive polozky jsou nasledne readonly dostupne jako dictionary.

```csharp
void Add(string key, object value);
```
#### Parameters

<a name='CIS.Infrastructure.CisMediatR.Rollback.IRollbackBag.Add(string,object).key'></a>

`key` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Unikatni klic polozky

<a name='CIS.Infrastructure.CisMediatR.Rollback.IRollbackBag.Add(string,object).value'></a>

`value` [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object')

Hodnota polozky - napr. int32 (id)
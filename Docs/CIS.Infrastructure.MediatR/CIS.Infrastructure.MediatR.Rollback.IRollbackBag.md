#### [CIS.Infrastructure.MediatR](index.md 'index')
### [CIS.Infrastructure.MediatR.Rollback](CIS.Infrastructure.MediatR.Rollback.md 'CIS.Infrastructure.MediatR.Rollback')

## IRollbackBag Interface

Uloziste umoznujici prenos vybranych dat z Meditr handleru do rollback handleru.

```csharp
public interface IRollbackBag
```
### Properties

<a name='CIS.Infrastructure.MediatR.Rollback.IRollbackBag.Count'></a>

## IRollbackBag.Count Property

Pocet polozek vlozenych do bagu

```csharp
int Count { get; }
```

#### Property Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')
### Methods

<a name='CIS.Infrastructure.MediatR.Rollback.IRollbackBag.Add(string,object)'></a>

## IRollbackBag.Add(string, object) Method

Prida dalsi polozku do bagu. Jednotlive polozky jsou nasledne readonly dostupne jako dictionary.

```csharp
void Add(string key, object value);
```
#### Parameters

<a name='CIS.Infrastructure.MediatR.Rollback.IRollbackBag.Add(string,object).key'></a>

`key` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Unikatni klic polozky

<a name='CIS.Infrastructure.MediatR.Rollback.IRollbackBag.Add(string,object).value'></a>

`value` [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object')

Hodnota polozky - napr. int32 (id)
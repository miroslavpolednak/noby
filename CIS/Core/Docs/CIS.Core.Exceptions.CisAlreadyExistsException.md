#### [CIS.Core](index.md 'index')
### [CIS.Core.Exceptions](CIS.Core.Exceptions.md 'CIS.Core.Exceptions')

## CisAlreadyExistsException Class

Objekt již existuje.

```csharp
public sealed class CisAlreadyExistsException : CIS.Core.Exceptions.BaseCisException
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; [System.Exception](https://docs.microsoft.com/en-us/dotnet/api/System.Exception 'System.Exception') &#129106; [CIS.Core.Exceptions.BaseCisException](https://docs.microsoft.com/en-us/dotnet/api/CIS.Core.Exceptions.BaseCisException 'CIS.Core.Exceptions.BaseCisException') &#129106; CisAlreadyExistsException

### Remarks
Např. pokud vytvářím entitu v databázi, ale toto ID již existuje. Nebo pokud přidávám do kolekce již existující klíč.
### Properties

<a name='CIS.Core.Exceptions.CisAlreadyExistsException.EntityId'></a>

## CisAlreadyExistsException.EntityId Property

Id entity, která vyvolala vyjímku

```csharp
public object? EntityId { get; set; }
```

#### Property Value
[System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object')

### Example
111

<a name='CIS.Core.Exceptions.CisAlreadyExistsException.EntityName'></a>

## CisAlreadyExistsException.EntityName Property

Název entity

```csharp
public string? EntityName { get; set; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

### Example
DomainServices.Api.TestClass
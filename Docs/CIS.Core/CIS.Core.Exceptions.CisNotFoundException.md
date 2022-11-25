#### [CIS.Core](index.md 'index')
### [CIS.Core.Exceptions](CIS.Core.Exceptions.md 'CIS.Core.Exceptions')

## CisNotFoundException Class

Objekt nebyl nalezen.

```csharp
public sealed class CisNotFoundException : CIS.Core.Exceptions.BaseCisException
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; [System.Exception](https://docs.microsoft.com/en-us/dotnet/api/System.Exception 'System.Exception') &#129106; [BaseCisException](CIS.Core.Exceptions.BaseCisException.md 'CIS.Core.Exceptions.BaseCisException') &#129106; CisNotFoundException

### Remarks
Např. při dotazu do databáze dané ID neexistuje.
### Constructors

<a name='CIS.Core.Exceptions.CisNotFoundException.CisNotFoundException(int,string,object)'></a>

## CisNotFoundException(int, string, object) Constructor

```csharp
public CisNotFoundException(int exceptionCode, string entityName, object entityId);
```
#### Parameters

<a name='CIS.Core.Exceptions.CisNotFoundException.CisNotFoundException(int,string,object).exceptionCode'></a>

`exceptionCode` [System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

CIS kód chyby

<a name='CIS.Core.Exceptions.CisNotFoundException.CisNotFoundException(int,string,object).entityName'></a>

`entityName` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Název entity, která chybu vyvolala

<a name='CIS.Core.Exceptions.CisNotFoundException.CisNotFoundException(int,string,object).entityId'></a>

`entityId` [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object')

ID entity, která chybu vyvolala

<a name='CIS.Core.Exceptions.CisNotFoundException.CisNotFoundException(int,string)'></a>

## CisNotFoundException(int, string) Constructor

```csharp
public CisNotFoundException(int exceptionCode, string message);
```
#### Parameters

<a name='CIS.Core.Exceptions.CisNotFoundException.CisNotFoundException(int,string).exceptionCode'></a>

`exceptionCode` [System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

CIS kód chyby

<a name='CIS.Core.Exceptions.CisNotFoundException.CisNotFoundException(int,string).message'></a>

`message` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Text chyby
### Properties

<a name='CIS.Core.Exceptions.CisNotFoundException.EntityId'></a>

## CisNotFoundException.EntityId Property

Id entity, která vyvolala vyjímku

```csharp
public object? EntityId { get; set; }
```

#### Property Value
[System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object')

### Example
111

<a name='CIS.Core.Exceptions.CisNotFoundException.EntityName'></a>

## CisNotFoundException.EntityName Property

Název entity

```csharp
public string? EntityName { get; set; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

### Example
DomainServices.Api.TestClass
#### [CIS.Core](index.md 'index')
### [CIS.Core.Exceptions](CIS.Core.Exceptions.md 'CIS.Core.Exceptions')

## CisConflictException Class

HTTP 409. Vyhazovat pokud prováděná akce je v konfliktu s existující byznys logikou. Podporuje kolekci chybových hlášení.

```csharp
public sealed class CisConflictException : CIS.Core.Exceptions.BaseCisException
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; [System.Exception](https://docs.microsoft.com/en-us/dotnet/api/System.Exception 'System.Exception') &#129106; [BaseCisException](CIS.Core.Exceptions.BaseCisException.md 'CIS.Core.Exceptions.BaseCisException') &#129106; CisConflictException

### Remarks
Např. pokud mám vrátit detail klienta, ale v CM je více klientů se stejným ID.
### Constructors

<a name='CIS.Core.Exceptions.CisConflictException.CisConflictException(int,string)'></a>

## CisConflictException(int, string) Constructor

```csharp
public CisConflictException(int exceptionCode, string message);
```
#### Parameters

<a name='CIS.Core.Exceptions.CisConflictException.CisConflictException(int,string).exceptionCode'></a>

`exceptionCode` [System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

CIS error kód

<a name='CIS.Core.Exceptions.CisConflictException.CisConflictException(int,string).message'></a>

`message` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Chybová zpráva

<a name='CIS.Core.Exceptions.CisConflictException.CisConflictException(string)'></a>

## CisConflictException(string) Constructor

```csharp
public CisConflictException(string message);
```
#### Parameters

<a name='CIS.Core.Exceptions.CisConflictException.CisConflictException(string).message'></a>

`message` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Chybová zpráva
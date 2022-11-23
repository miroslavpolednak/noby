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
### Properties

<a name='CIS.Core.Exceptions.CisConflictException.Errors'></a>

## CisConflictException.Errors Property

Seznam chyb.

```csharp
public System.Collections.Immutable.IImmutableList<(string Key,string Message)>? Errors { get; set; }
```

#### Property Value
[System.Collections.Immutable.IImmutableList&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Immutable.IImmutableList-1 'System.Collections.Immutable.IImmutableList`1')[&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.ValueTuple 'System.ValueTuple')[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')[,](https://docs.microsoft.com/en-us/dotnet/api/System.ValueTuple 'System.ValueTuple')[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.ValueTuple 'System.ValueTuple')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Immutable.IImmutableList-1 'System.Collections.Immutable.IImmutableList`1')

### Remarks
Key: CIS error kód <br/>  
Message: chybová zpráva
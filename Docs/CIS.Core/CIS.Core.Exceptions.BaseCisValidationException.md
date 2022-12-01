#### [CIS.Core](index.md 'index')
### [CIS.Core.Exceptions](CIS.Core.Exceptions.md 'CIS.Core.Exceptions')

## BaseCisValidationException Class

```csharp
public abstract class BaseCisValidationException : CIS.Core.Exceptions.BaseCisException
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; [System.Exception](https://docs.microsoft.com/en-us/dotnet/api/System.Exception 'System.Exception') &#129106; [BaseCisException](CIS.Core.Exceptions.BaseCisException.md 'CIS.Core.Exceptions.BaseCisException') &#129106; BaseCisValidationException

Derived  
&#8627; [CisValidationException](CIS.Core.Exceptions.CisValidationException.md 'CIS.Core.Exceptions.CisValidationException')  
&#8627; [CisExtServiceValidationException](CIS.Core.Exceptions.ExternalServices.CisExtServiceValidationException.md 'CIS.Core.Exceptions.ExternalServices.CisExtServiceValidationException')
### Constructors

<a name='CIS.Core.Exceptions.BaseCisValidationException.BaseCisValidationException(int,string)'></a>

## BaseCisValidationException(int, string) Constructor

```csharp
public BaseCisValidationException(int exceptionCode, string message);
```
#### Parameters

<a name='CIS.Core.Exceptions.BaseCisValidationException.BaseCisValidationException(int,string).exceptionCode'></a>

`exceptionCode` [System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

CIS error kód

<a name='CIS.Core.Exceptions.BaseCisValidationException.BaseCisValidationException(int,string).message'></a>

`message` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Chybová zpráva
### Properties

<a name='CIS.Core.Exceptions.BaseCisValidationException.Errors'></a>

## BaseCisValidationException.Errors Property

Seznam chyb.

```csharp
public System.Collections.Immutable.ImmutableList<(string Key,string Message)>? Errors { get; set; }
```

#### Property Value
[System.Collections.Immutable.ImmutableList&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Immutable.ImmutableList-1 'System.Collections.Immutable.ImmutableList`1')[&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.ValueTuple 'System.ValueTuple')[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')[,](https://docs.microsoft.com/en-us/dotnet/api/System.ValueTuple 'System.ValueTuple')[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.ValueTuple 'System.ValueTuple')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Immutable.ImmutableList-1 'System.Collections.Immutable.ImmutableList`1')

### Remarks
Key: CIS error kód <br/>  
Message: chybová zpráva
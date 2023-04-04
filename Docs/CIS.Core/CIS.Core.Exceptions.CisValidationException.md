#### [CIS.Core](index.md 'index')
### [CIS.Core.Exceptions](CIS.Core.Exceptions.md 'CIS.Core.Exceptions')

## CisValidationException Class

Validační chyba.

```csharp
public class CisValidationException : System.Exception
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; [System.Exception](https://docs.microsoft.com/en-us/dotnet/api/System.Exception 'System.Exception') &#129106; CisValidationException

Derived  
&#8627; [CisExtServiceValidationException](CIS.Core.Exceptions.ExternalServices.CisExtServiceValidationException.md 'CIS.Core.Exceptions.ExternalServices.CisExtServiceValidationException')

### Remarks
Vyhazujeme v případě, že chceme propagovat ošetřené chyby v byznys logice - primárně z FluentValidation.  
Může také ošetřovat stavy místo ArgumentException nebo ArgumentNullException a podobně.
### Constructors

<a name='CIS.Core.Exceptions.CisValidationException.CisValidationException(int,string)'></a>

## CisValidationException(int, string) Constructor

```csharp
public CisValidationException(int exceptionCode, string message);
```
#### Parameters

<a name='CIS.Core.Exceptions.CisValidationException.CisValidationException(int,string).exceptionCode'></a>

`exceptionCode` [System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

CIS error kód

<a name='CIS.Core.Exceptions.CisValidationException.CisValidationException(int,string).message'></a>

`message` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Chybová zpráva

<a name='CIS.Core.Exceptions.CisValidationException.CisValidationException(string,string)'></a>

## CisValidationException(string, string) Constructor

```csharp
public CisValidationException(string exceptionCode, string message);
```
#### Parameters

<a name='CIS.Core.Exceptions.CisValidationException.CisValidationException(string,string).exceptionCode'></a>

`exceptionCode` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

CIS error kód

<a name='CIS.Core.Exceptions.CisValidationException.CisValidationException(string,string).message'></a>

`message` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Chybová zpráva

<a name='CIS.Core.Exceptions.CisValidationException.CisValidationException(string)'></a>

## CisValidationException(string) Constructor

```csharp
public CisValidationException(string message);
```
#### Parameters

<a name='CIS.Core.Exceptions.CisValidationException.CisValidationException(string).message'></a>

`message` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Chybová zpráva

<a name='CIS.Core.Exceptions.CisValidationException.CisValidationException(System.Collections.Generic.IEnumerable_CIS.Core.Exceptions.CisExceptionItem_)'></a>

## CisValidationException(IEnumerable<CisExceptionItem>) Constructor

```csharp
public CisValidationException(System.Collections.Generic.IEnumerable<CIS.Core.Exceptions.CisExceptionItem> errors);
```
#### Parameters

<a name='CIS.Core.Exceptions.CisValidationException.CisValidationException(System.Collections.Generic.IEnumerable_CIS.Core.Exceptions.CisExceptionItem_).errors'></a>

`errors` [System.Collections.Generic.IEnumerable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.IEnumerable-1 'System.Collections.Generic.IEnumerable`1')[CisExceptionItem](CIS.Core.Exceptions.CisExceptionItem.md 'CIS.Core.Exceptions.CisExceptionItem')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.IEnumerable-1 'System.Collections.Generic.IEnumerable`1')

Seznam chyb

<a name='CIS.Core.Exceptions.CisValidationException.CisValidationException(System.Collections.Generic.IReadOnlyList_CIS.Core.Exceptions.CisExceptionItem_)'></a>

## CisValidationException(IReadOnlyList<CisExceptionItem>) Constructor

```csharp
public CisValidationException(System.Collections.Generic.IReadOnlyList<CIS.Core.Exceptions.CisExceptionItem> errors);
```
#### Parameters

<a name='CIS.Core.Exceptions.CisValidationException.CisValidationException(System.Collections.Generic.IReadOnlyList_CIS.Core.Exceptions.CisExceptionItem_).errors'></a>

`errors` [System.Collections.Generic.IReadOnlyList&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.IReadOnlyList-1 'System.Collections.Generic.IReadOnlyList`1')[CisExceptionItem](CIS.Core.Exceptions.CisExceptionItem.md 'CIS.Core.Exceptions.CisExceptionItem')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.IReadOnlyList-1 'System.Collections.Generic.IReadOnlyList`1')

Seznam chyb
### Properties

<a name='CIS.Core.Exceptions.CisValidationException.Errors'></a>

## CisValidationException.Errors Property

Seznam chyb.

```csharp
public System.Collections.Generic.IReadOnlyList<CIS.Core.Exceptions.CisExceptionItem> Errors { get; set; }
```

#### Property Value
[System.Collections.Generic.IReadOnlyList&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.IReadOnlyList-1 'System.Collections.Generic.IReadOnlyList`1')[CisExceptionItem](CIS.Core.Exceptions.CisExceptionItem.md 'CIS.Core.Exceptions.CisExceptionItem')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.IReadOnlyList-1 'System.Collections.Generic.IReadOnlyList`1')
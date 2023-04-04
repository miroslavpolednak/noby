#### [CIS.Core](index.md 'index')
### [CIS.Core.Exceptions.ExternalServices](CIS.Core.Exceptions.ExternalServices.md 'CIS.Core.Exceptions.ExternalServices')

## CisExtServiceValidationException Class

HTTP 400. Chyba, která vzniká při volání API třetích stran. Pokud API vrátí HTTP 4xx, vytáhneme z odpovědi chybu a vyvoláme tuto vyjímku. Podporuje seznam chyb.

```csharp
public sealed class CisExtServiceValidationException : CIS.Core.Exceptions.CisValidationException
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; [System.Exception](https://docs.microsoft.com/en-us/dotnet/api/System.Exception 'System.Exception') &#129106; [CisValidationException](CIS.Core.Exceptions.CisValidationException.md 'CIS.Core.Exceptions.CisValidationException') &#129106; CisExtServiceValidationException
### Constructors

<a name='CIS.Core.Exceptions.ExternalServices.CisExtServiceValidationException.CisExtServiceValidationException(int,string)'></a>

## CisExtServiceValidationException(int, string) Constructor

```csharp
public CisExtServiceValidationException(int exceptionCode, string message);
```
#### Parameters

<a name='CIS.Core.Exceptions.ExternalServices.CisExtServiceValidationException.CisExtServiceValidationException(int,string).exceptionCode'></a>

`exceptionCode` [System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

CIS error kód

<a name='CIS.Core.Exceptions.ExternalServices.CisExtServiceValidationException.CisExtServiceValidationException(int,string).message'></a>

`message` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Chybová zpráva

<a name='CIS.Core.Exceptions.ExternalServices.CisExtServiceValidationException.CisExtServiceValidationException(string,string)'></a>

## CisExtServiceValidationException(string, string) Constructor

```csharp
public CisExtServiceValidationException(string exceptionCode, string message);
```
#### Parameters

<a name='CIS.Core.Exceptions.ExternalServices.CisExtServiceValidationException.CisExtServiceValidationException(string,string).exceptionCode'></a>

`exceptionCode` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

CIS error kód

<a name='CIS.Core.Exceptions.ExternalServices.CisExtServiceValidationException.CisExtServiceValidationException(string,string).message'></a>

`message` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Chybová zpráva

<a name='CIS.Core.Exceptions.ExternalServices.CisExtServiceValidationException.CisExtServiceValidationException(string)'></a>

## CisExtServiceValidationException(string) Constructor

```csharp
public CisExtServiceValidationException(string message);
```
#### Parameters

<a name='CIS.Core.Exceptions.ExternalServices.CisExtServiceValidationException.CisExtServiceValidationException(string).message'></a>

`message` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Chybová zpráva

<a name='CIS.Core.Exceptions.ExternalServices.CisExtServiceValidationException.CisExtServiceValidationException(System.Collections.Generic.IEnumerable_CIS.Core.Exceptions.CisExceptionItem_)'></a>

## CisExtServiceValidationException(IEnumerable<CisExceptionItem>) Constructor

```csharp
public CisExtServiceValidationException(System.Collections.Generic.IEnumerable<CIS.Core.Exceptions.CisExceptionItem> errors);
```
#### Parameters

<a name='CIS.Core.Exceptions.ExternalServices.CisExtServiceValidationException.CisExtServiceValidationException(System.Collections.Generic.IEnumerable_CIS.Core.Exceptions.CisExceptionItem_).errors'></a>

`errors` [System.Collections.Generic.IEnumerable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.IEnumerable-1 'System.Collections.Generic.IEnumerable`1')[CisExceptionItem](CIS.Core.Exceptions.CisExceptionItem.md 'CIS.Core.Exceptions.CisExceptionItem')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.IEnumerable-1 'System.Collections.Generic.IEnumerable`1')

Seznam chyb
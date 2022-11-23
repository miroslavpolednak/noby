#### [CIS.Core](index.md 'index')
### [CIS.Core.Exceptions](CIS.Core.Exceptions.md 'CIS.Core.Exceptions')

## CisValidationException Class

HTTP 400. Validační chyba.

```csharp
public sealed class CisValidationException : CIS.Core.Exceptions.BaseCisValidationException
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; [System.Exception](https://docs.microsoft.com/en-us/dotnet/api/System.Exception 'System.Exception') &#129106; [BaseCisException](CIS.Core.Exceptions.BaseCisException.md 'CIS.Core.Exceptions.BaseCisException') &#129106; [BaseCisValidationException](CIS.Core.Exceptions.BaseCisValidationException.md 'CIS.Core.Exceptions.BaseCisValidationException') &#129106; CisValidationException

### Remarks
Vyhazujeme v případě, že chceme propagovat ošetřené chyby v byznys logice - primárně z FluentValidation. Podporuje seznam chyb.
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

<a name='CIS.Core.Exceptions.CisValidationException.CisValidationException(string)'></a>

## CisValidationException(string) Constructor

```csharp
public CisValidationException(string message);
```
#### Parameters

<a name='CIS.Core.Exceptions.CisValidationException.CisValidationException(string).message'></a>

`message` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Chybová zpráva
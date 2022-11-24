#### [CIS.Core](index.md 'index')
### [CIS.Core.Exceptions](CIS.Core.Exceptions.md 'CIS.Core.Exceptions')

## BaseCisException Class

Base třída pro CIS vyjímky. Obsahuje vlastnost ExceptionCode, která určuje o jakou vyjímku se jedná.

```csharp
public abstract class BaseCisException : System.Exception
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; [System.Exception](https://docs.microsoft.com/en-us/dotnet/api/System.Exception 'System.Exception') &#129106; BaseCisException

Derived  
&#8627; [BaseCisValidationException](CIS.Core.Exceptions.BaseCisValidationException.md 'CIS.Core.Exceptions.BaseCisValidationException')  
&#8627; [CisAlreadyExistsException](CIS.Core.Exceptions.CisAlreadyExistsException.md 'CIS.Core.Exceptions.CisAlreadyExistsException')  
&#8627; [CisConfigurationException](CIS.Core.Exceptions.CisConfigurationException.md 'CIS.Core.Exceptions.CisConfigurationException')  
&#8627; [CisConfigurationNotFound](CIS.Core.Exceptions.CisConfigurationNotFound.md 'CIS.Core.Exceptions.CisConfigurationNotFound')  
&#8627; [CisConfigurationNotRegisteredException](CIS.Core.Exceptions.CisConfigurationNotRegisteredException.md 'CIS.Core.Exceptions.CisConfigurationNotRegisteredException')  
&#8627; [CisConflictException](CIS.Core.Exceptions.CisConflictException.md 'CIS.Core.Exceptions.CisConflictException')  
&#8627; [CisException](CIS.Core.Exceptions.CisException.md 'CIS.Core.Exceptions.CisException')  
&#8627; [CisExtServiceResponseDeserializationException](CIS.Core.Exceptions.CisExtServiceResponseDeserializationException.md 'CIS.Core.Exceptions.CisExtServiceResponseDeserializationException')  
&#8627; [CisNotFoundException](CIS.Core.Exceptions.CisNotFoundException.md 'CIS.Core.Exceptions.CisNotFoundException')  
&#8627; [CisServiceCallResultErrorException](CIS.Core.Exceptions.CisServiceCallResultErrorException.md 'CIS.Core.Exceptions.CisServiceCallResultErrorException')  
&#8627; [CisServiceServerErrorException](CIS.Core.Exceptions.CisServiceServerErrorException.md 'CIS.Core.Exceptions.CisServiceServerErrorException')  
&#8627; [CisServiceUnavailableException](CIS.Core.Exceptions.CisServiceUnavailableException.md 'CIS.Core.Exceptions.CisServiceUnavailableException')
### Constructors

<a name='CIS.Core.Exceptions.BaseCisException.BaseCisException(int,string)'></a>

## BaseCisException(int, string) Constructor

```csharp
public BaseCisException(int exceptionCode, string? message);
```
#### Parameters

<a name='CIS.Core.Exceptions.BaseCisException.BaseCisException(int,string).exceptionCode'></a>

`exceptionCode` [System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

CIS error kód

<a name='CIS.Core.Exceptions.BaseCisException.BaseCisException(int,string).message'></a>

`message` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Chybová zpráva
### Fields

<a name='CIS.Core.Exceptions.BaseCisException.UnknownExceptionCode'></a>

## BaseCisException.UnknownExceptionCode Field

Společný kód pro neznámou chybu

```csharp
public const int UnknownExceptionCode = 0;
```

#### Field Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')
### Properties

<a name='CIS.Core.Exceptions.BaseCisException.ExceptionCode'></a>

## BaseCisException.ExceptionCode Property

CIS error kód

```csharp
public int ExceptionCode { get; set; }
```

#### Property Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')
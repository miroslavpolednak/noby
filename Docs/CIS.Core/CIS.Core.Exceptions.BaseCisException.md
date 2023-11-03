#### [CIS.Core](index.md 'index')
### [CIS.Core.Exceptions](CIS.Core.Exceptions.md 'CIS.Core.Exceptions')

## BaseCisException Class

Base třída pro CIS vyjímky. Obsahuje vlastnost ExceptionCode, která určuje o jakou vyjímku se jedná.

```csharp
public abstract class BaseCisException : System.Exception
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; [System.Exception](https://docs.microsoft.com/en-us/dotnet/api/System.Exception 'System.Exception') &#129106; BaseCisException

Derived  
&#8627; [CisAlreadyExistsException](CIS.Core.Exceptions.CisAlreadyExistsException.md 'CIS.Core.Exceptions.CisAlreadyExistsException')  
&#8627; [CisConfigurationException](CIS.Core.Exceptions.CisConfigurationException.md 'CIS.Core.Exceptions.CisConfigurationException')  
&#8627; [CisConfigurationNotFound](CIS.Core.Exceptions.CisConfigurationNotFound.md 'CIS.Core.Exceptions.CisConfigurationNotFound')  
&#8627; [CisConfigurationNotRegisteredException](CIS.Core.Exceptions.CisConfigurationNotRegisteredException.md 'CIS.Core.Exceptions.CisConfigurationNotRegisteredException')  
&#8627; [CisException](CIS.Core.Exceptions.CisException.md 'CIS.Core.Exceptions.CisException')  
&#8627; [CisInvalidApplicationKeyException](CIS.Core.Exceptions.CisInvalidApplicationKeyException.md 'CIS.Core.Exceptions.CisInvalidApplicationKeyException')  
&#8627; [CisInvalidEnvironmentNameException](CIS.Core.Exceptions.CisInvalidEnvironmentNameException.md 'CIS.Core.Exceptions.CisInvalidEnvironmentNameException')  
&#8627; [CisNotFoundException](CIS.Core.Exceptions.CisNotFoundException.md 'CIS.Core.Exceptions.CisNotFoundException')  
&#8627; [CisServiceServerErrorException](CIS.Core.Exceptions.CisServiceServerErrorException.md 'CIS.Core.Exceptions.CisServiceServerErrorException')  
&#8627; [CisServiceUnavailableException](CIS.Core.Exceptions.CisServiceUnavailableException.md 'CIS.Core.Exceptions.CisServiceUnavailableException')  
&#8627; [CisExtServiceResponseDeserializationException](CIS.Core.Exceptions.ExternalServices.CisExtServiceResponseDeserializationException.md 'CIS.Core.Exceptions.ExternalServices.CisExtServiceResponseDeserializationException')  
&#8627; [CisExtServiceServerErrorException](CIS.Core.Exceptions.ExternalServices.CisExtServiceServerErrorException.md 'CIS.Core.Exceptions.ExternalServices.CisExtServiceServerErrorException')  
&#8627; [CisExtServiceUnavailableException](CIS.Core.Exceptions.ExternalServices.CisExtServiceUnavailableException.md 'CIS.Core.Exceptions.ExternalServices.CisExtServiceUnavailableException')
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

<a name='CIS.Core.Exceptions.BaseCisException.BaseCisException(string,string)'></a>

## BaseCisException(string, string) Constructor

```csharp
public BaseCisException(string exceptionCode, string? message);
```
#### Parameters

<a name='CIS.Core.Exceptions.BaseCisException.BaseCisException(string,string).exceptionCode'></a>

`exceptionCode` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

CIS error kód

<a name='CIS.Core.Exceptions.BaseCisException.BaseCisException(string,string).message'></a>

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
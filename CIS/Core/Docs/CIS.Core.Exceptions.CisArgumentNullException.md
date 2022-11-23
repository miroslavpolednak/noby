#### [CIS.Core](index.md 'index')
### [CIS.Core.Exceptions](CIS.Core.Exceptions.md 'CIS.Core.Exceptions')

## CisArgumentNullException Class

Stejná chyba jako [System.ArgumentNullException](https://docs.microsoft.com/en-us/dotnet/api/System.ArgumentNullException 'System.ArgumentNullException'), ale obsahuje navíc CIS error kód

```csharp
public sealed class CisArgumentNullException : System.ArgumentNullException
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; [System.Exception](https://docs.microsoft.com/en-us/dotnet/api/System.Exception 'System.Exception') &#129106; [System.SystemException](https://docs.microsoft.com/en-us/dotnet/api/System.SystemException 'System.SystemException') &#129106; [System.ArgumentException](https://docs.microsoft.com/en-us/dotnet/api/System.ArgumentException 'System.ArgumentException') &#129106; [System.ArgumentNullException](https://docs.microsoft.com/en-us/dotnet/api/System.ArgumentNullException 'System.ArgumentNullException') &#129106; CisArgumentNullException
### Constructors

<a name='CIS.Core.Exceptions.CisArgumentNullException.CisArgumentNullException(int,string,string)'></a>

## CisArgumentNullException(int, string, string) Constructor

```csharp
public CisArgumentNullException(int exceptionCode, string message, string paramName);
```
#### Parameters

<a name='CIS.Core.Exceptions.CisArgumentNullException.CisArgumentNullException(int,string,string).exceptionCode'></a>

`exceptionCode` [System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

CIS error kód

<a name='CIS.Core.Exceptions.CisArgumentNullException.CisArgumentNullException(int,string,string).message'></a>

`message` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Chybová zpráva

<a name='CIS.Core.Exceptions.CisArgumentNullException.CisArgumentNullException(int,string,string).paramName'></a>

`paramName` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Název parametru, který chybu vyvolal
### Properties

<a name='CIS.Core.Exceptions.CisArgumentNullException.ExceptionCode'></a>

## CisArgumentNullException.ExceptionCode Property

CIS error kód

```csharp
public int ExceptionCode { get; set; }
```

#### Property Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')
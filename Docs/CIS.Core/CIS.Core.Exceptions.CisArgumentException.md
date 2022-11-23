#### [CIS.Core](index.md 'index')
### [CIS.Core.Exceptions](CIS.Core.Exceptions.md 'CIS.Core.Exceptions')

## CisArgumentException Class

Stejná chyba jako [System.ArgumentException](https://docs.microsoft.com/en-us/dotnet/api/System.ArgumentException 'System.ArgumentException'), ale obsahuje navíc CIS error kód

```csharp
public sealed class CisArgumentException : CIS.Core.Exceptions.BaseCisArgumentException
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; [System.Exception](https://docs.microsoft.com/en-us/dotnet/api/System.Exception 'System.Exception') &#129106; [System.SystemException](https://docs.microsoft.com/en-us/dotnet/api/System.SystemException 'System.SystemException') &#129106; [System.ArgumentException](https://docs.microsoft.com/en-us/dotnet/api/System.ArgumentException 'System.ArgumentException') &#129106; [BaseCisArgumentException](CIS.Core.Exceptions.BaseCisArgumentException.md 'CIS.Core.Exceptions.BaseCisArgumentException') &#129106; CisArgumentException
### Constructors

<a name='CIS.Core.Exceptions.CisArgumentException.CisArgumentException(int,string,string)'></a>

## CisArgumentException(int, string, string) Constructor

```csharp
public CisArgumentException(int exceptionCode, string message, string paramName);
```
#### Parameters

<a name='CIS.Core.Exceptions.CisArgumentException.CisArgumentException(int,string,string).exceptionCode'></a>

`exceptionCode` [System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

CIS error kód

<a name='CIS.Core.Exceptions.CisArgumentException.CisArgumentException(int,string,string).message'></a>

`message` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Chybová zpráva

<a name='CIS.Core.Exceptions.CisArgumentException.CisArgumentException(int,string,string).paramName'></a>

`paramName` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Název parametru, který chybu vyvolal
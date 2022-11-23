#### [CIS.Core](index.md 'index')
### [CIS.Core.Exceptions](CIS.Core.Exceptions.md 'CIS.Core.Exceptions')

## CisInvalidApplicationKeyException Class

Chyba validace názvu aplikace - vyvoláno z konstruktoru value type ApplicationKey

```csharp
public sealed class CisInvalidApplicationKeyException : CIS.Core.Exceptions.BaseCisArgumentException
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; [System.Exception](https://docs.microsoft.com/en-us/dotnet/api/System.Exception 'System.Exception') &#129106; [System.SystemException](https://docs.microsoft.com/en-us/dotnet/api/System.SystemException 'System.SystemException') &#129106; [System.ArgumentException](https://docs.microsoft.com/en-us/dotnet/api/System.ArgumentException 'System.ArgumentException') &#129106; [BaseCisArgumentException](CIS.Core.Exceptions.BaseCisArgumentException.md 'CIS.Core.Exceptions.BaseCisArgumentException') &#129106; CisInvalidApplicationKeyException
### Constructors

<a name='CIS.Core.Exceptions.CisInvalidApplicationKeyException.CisInvalidApplicationKeyException(string)'></a>

## CisInvalidApplicationKeyException(string) Constructor

```csharp
public CisInvalidApplicationKeyException(string key);
```
#### Parameters

<a name='CIS.Core.Exceptions.CisInvalidApplicationKeyException.CisInvalidApplicationKeyException(string).key'></a>

`key` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Název aplikace

<a name='CIS.Core.Exceptions.CisInvalidApplicationKeyException.CisInvalidApplicationKeyException(string,string)'></a>

## CisInvalidApplicationKeyException(string, string) Constructor

```csharp
public CisInvalidApplicationKeyException(string key, string paramName);
```
#### Parameters

<a name='CIS.Core.Exceptions.CisInvalidApplicationKeyException.CisInvalidApplicationKeyException(string,string).key'></a>

`key` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Název aplikace

<a name='CIS.Core.Exceptions.CisInvalidApplicationKeyException.CisInvalidApplicationKeyException(string,string).paramName'></a>

`paramName` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')
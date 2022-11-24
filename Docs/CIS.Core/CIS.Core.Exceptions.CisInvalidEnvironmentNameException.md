#### [CIS.Core](index.md 'index')
### [CIS.Core.Exceptions](CIS.Core.Exceptions.md 'CIS.Core.Exceptions')

## CisInvalidEnvironmentNameException Class

Chyba validace názvu prostředí - vyvoláno z konstruktoru value type EnvironmentName

```csharp
public sealed class CisInvalidEnvironmentNameException : CIS.Core.Exceptions.BaseCisArgumentException
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; [System.Exception](https://docs.microsoft.com/en-us/dotnet/api/System.Exception 'System.Exception') &#129106; [System.SystemException](https://docs.microsoft.com/en-us/dotnet/api/System.SystemException 'System.SystemException') &#129106; [System.ArgumentException](https://docs.microsoft.com/en-us/dotnet/api/System.ArgumentException 'System.ArgumentException') &#129106; [BaseCisArgumentException](CIS.Core.Exceptions.BaseCisArgumentException.md 'CIS.Core.Exceptions.BaseCisArgumentException') &#129106; CisInvalidEnvironmentNameException
### Constructors

<a name='CIS.Core.Exceptions.CisInvalidEnvironmentNameException.CisInvalidEnvironmentNameException(string)'></a>

## CisInvalidEnvironmentNameException(string) Constructor

```csharp
public CisInvalidEnvironmentNameException(string name);
```
#### Parameters

<a name='CIS.Core.Exceptions.CisInvalidEnvironmentNameException.CisInvalidEnvironmentNameException(string).name'></a>

`name` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Název prostředí

<a name='CIS.Core.Exceptions.CisInvalidEnvironmentNameException.CisInvalidEnvironmentNameException(string,string)'></a>

## CisInvalidEnvironmentNameException(string, string) Constructor

```csharp
public CisInvalidEnvironmentNameException(string name, string paramName);
```
#### Parameters

<a name='CIS.Core.Exceptions.CisInvalidEnvironmentNameException.CisInvalidEnvironmentNameException(string,string).name'></a>

`name` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Název prostředí

<a name='CIS.Core.Exceptions.CisInvalidEnvironmentNameException.CisInvalidEnvironmentNameException(string,string).paramName'></a>

`paramName` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Název parametru ve kterém bylo předáno prostředí
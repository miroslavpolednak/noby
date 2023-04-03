#### [CIS.Core](index.md 'index')
### [CIS.Core.Exceptions](CIS.Core.Exceptions.md 'CIS.Core.Exceptions')

## CisInvalidApplicationKeyException Class

Chyba validace názvu aplikace - vyvoláno z konstruktoru value type ApplicationKey

```csharp
public sealed class CisInvalidApplicationKeyException : CIS.Core.Exceptions.BaseCisException
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; [System.Exception](https://docs.microsoft.com/en-us/dotnet/api/System.Exception 'System.Exception') &#129106; [BaseCisException](CIS.Core.Exceptions.BaseCisException.md 'CIS.Core.Exceptions.BaseCisException') &#129106; CisInvalidApplicationKeyException
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
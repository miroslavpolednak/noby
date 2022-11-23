#### [CIS.Core](index.md 'index')
### [CIS.Core.Exceptions](CIS.Core.Exceptions.md 'CIS.Core.Exceptions')

## CisConfigurationNotFound Class

Pokud chybí požadované nastavení konfigurace v appsettings.json

```csharp
public sealed class CisConfigurationNotFound : CIS.Core.Exceptions.BaseCisException
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; [System.Exception](https://docs.microsoft.com/en-us/dotnet/api/System.Exception 'System.Exception') &#129106; [BaseCisException](CIS.Core.Exceptions.BaseCisException.md 'CIS.Core.Exceptions.BaseCisException') &#129106; CisConfigurationNotFound
### Constructors

<a name='CIS.Core.Exceptions.CisConfigurationNotFound.CisConfigurationNotFound(string)'></a>

## CisConfigurationNotFound(string) Constructor

```csharp
public CisConfigurationNotFound(string sectionName);
```
#### Parameters

<a name='CIS.Core.Exceptions.CisConfigurationNotFound.CisConfigurationNotFound(string).sectionName'></a>

`sectionName` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Název sekce v appsettings.json, která chybí
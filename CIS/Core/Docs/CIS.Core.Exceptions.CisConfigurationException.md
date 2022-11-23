#### [CIS.Core](index.md 'index')
### [CIS.Core.Exceptions](CIS.Core.Exceptions.md 'CIS.Core.Exceptions')

## CisConfigurationException Class

Pokud nastavení konfigurace v appsettings.json pro danou konfigurační sekci nelze zvalidovat - tj. chybí nastavit některá z required props atd.

```csharp
public sealed class CisConfigurationException : CIS.Core.Exceptions.BaseCisException
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; [System.Exception](https://docs.microsoft.com/en-us/dotnet/api/System.Exception 'System.Exception') &#129106; [BaseCisException](CIS.Core.Exceptions.BaseCisException.md 'CIS.Core.Exceptions.BaseCisException') &#129106; CisConfigurationException
### Constructors

<a name='CIS.Core.Exceptions.CisConfigurationException.CisConfigurationException(int,string)'></a>

## CisConfigurationException(int, string) Constructor

```csharp
public CisConfigurationException(int exceptionCode, string? message);
```
#### Parameters

<a name='CIS.Core.Exceptions.CisConfigurationException.CisConfigurationException(int,string).exceptionCode'></a>

`exceptionCode` [System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

CIS error kód

<a name='CIS.Core.Exceptions.CisConfigurationException.CisConfigurationException(int,string).message'></a>

`message` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Chybová zpráva
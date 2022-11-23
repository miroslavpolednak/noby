#### [CIS.Core](index.md 'index')
### [CIS.Core.Types](CIS.Core.Types.md 'CIS.Core.Types')

## ApplicationKey Class

Value type pro název aplikace / služby.

```csharp
public sealed class ApplicationKey :
System.IEquatable<CIS.Core.Types.ApplicationKey>
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; ApplicationKey

Implements [System.IEquatable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.IEquatable-1 'System.IEquatable`1')[ApplicationKey](CIS.Core.Types.ApplicationKey.md 'CIS.Core.Types.ApplicationKey')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.IEquatable-1 'System.IEquatable`1')
### Constructors

<a name='CIS.Core.Types.ApplicationKey.ApplicationKey(string)'></a>

## ApplicationKey(string) Constructor

```csharp
public ApplicationKey(string? key);
```
#### Parameters

<a name='CIS.Core.Types.ApplicationKey.ApplicationKey(string).key'></a>

`key` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Název aplikace {DS|CIS}:{app_name}

#### Exceptions

[CisInvalidApplicationKeyException](CIS.Core.Exceptions.CisInvalidApplicationKeyException.md 'CIS.Core.Exceptions.CisInvalidApplicationKeyException')  
Název aplikace není vyplněný nebo nemá povolený formát.
### Properties

<a name='CIS.Core.Types.ApplicationKey.Key'></a>

## ApplicationKey.Key Property

Název aplikace v prostředí NOBY.

```csharp
public string Key { get; set; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

### Remarks
DS:CustomerService
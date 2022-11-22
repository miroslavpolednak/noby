#### [CIS.Core](index.md 'index')
### [CIS.Core.Types](CIS.Core.Types.md 'CIS.Core.Types')

## ApplicationEnvironmentName Class

Value type pro název/typ aplikačního prostředí.

```csharp
public sealed class ApplicationEnvironmentName :
System.IEquatable<CIS.Core.Types.ApplicationEnvironmentName>
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; ApplicationEnvironmentName

Implements [System.IEquatable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.IEquatable-1 'System.IEquatable`1')[ApplicationEnvironmentName](CIS.Core.Types.ApplicationEnvironmentName.md 'CIS.Core.Types.ApplicationEnvironmentName')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.IEquatable-1 'System.IEquatable`1')
### Constructors

<a name='CIS.Core.Types.ApplicationEnvironmentName.ApplicationEnvironmentName(string)'></a>

## ApplicationEnvironmentName(string) Constructor

```csharp
public ApplicationEnvironmentName(string? environment);
```
#### Parameters

<a name='CIS.Core.Types.ApplicationEnvironmentName.ApplicationEnvironmentName(string).environment'></a>

`environment` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Název prostředí

#### Exceptions

[CIS.Core.Exceptions.CisInvalidEnvironmentNameException](https://docs.microsoft.com/en-us/dotnet/api/CIS.Core.Exceptions.CisInvalidEnvironmentNameException 'CIS.Core.Exceptions.CisInvalidEnvironmentNameException')  
Název prostředí není zadaný nebo nemá platný formát.
### Properties

<a name='CIS.Core.Types.ApplicationEnvironmentName.Name'></a>

## ApplicationEnvironmentName.Name Property

Název aplikačního prostředí.

```csharp
public string Name { get; set; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

### Example
FAT
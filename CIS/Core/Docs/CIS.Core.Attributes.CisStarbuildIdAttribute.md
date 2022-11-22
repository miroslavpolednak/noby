#### [CIS.Core](index.md 'index')
### [CIS.Core.Attributes](CIS.Core.Attributes.md 'CIS.Core.Attributes')

## CisStarbuildIdAttribute Class

Specifický atribut pro číselníkové enumy.

```csharp
public sealed class CisStarbuildIdAttribute : System.Attribute
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; [System.Attribute](https://docs.microsoft.com/en-us/dotnet/api/System.Attribute 'System.Attribute') &#129106; CisStarbuildIdAttribute

### Remarks
Označuje hodnotu dané enum value ve Starbuildu:  
`StarbuildId = t.GetAttribute<CIS.Core.Attributes.CisStarbuildIdAttribute>()?.StarbuildId`
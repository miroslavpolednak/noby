#### [CIS.Core](index.md 'index')
### [CIS.Core.Attributes](CIS.Core.Attributes.md 'CIS.Core.Attributes')

## CisDefaultValueAttribute Class

Používá se k označení výchozí hodnoty enumu.

```csharp
public sealed class CisDefaultValueAttribute : System.Attribute
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; [System.Attribute](https://docs.microsoft.com/en-us/dotnet/api/System.Attribute 'System.Attribute') &#129106; CisDefaultValueAttribute

### Remarks
Volající kód může podobným dotazem zjistit, zda je daná enum value výchozí:  
`IsDefault = t.HasAttribute<CIS.Core.Attributes.CisDefaultValueAttribute>()`
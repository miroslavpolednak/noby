#### [CIS.Core](index.md 'index')
### [CIS.Core.Exceptions.ExternalServices](CIS.Core.Exceptions.ExternalServices.md 'CIS.Core.Exceptions.ExternalServices')

## CisExtServiceResponseDeserializationException Class

Chyba, která vzniká při volání API třetích stran. Pokud se nepodaří deserializovat response volaného API, vyhodíme tuto vyjímku.

```csharp
public sealed class CisExtServiceResponseDeserializationException : CIS.Core.Exceptions.BaseCisException
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; [System.Exception](https://docs.microsoft.com/en-us/dotnet/api/System.Exception 'System.Exception') &#129106; [BaseCisException](CIS.Core.Exceptions.BaseCisException.md 'CIS.Core.Exceptions.BaseCisException') &#129106; CisExtServiceResponseDeserializationException

### Remarks
Vznikne, pokud API třetí strany vrátí jiný JSON, než jaký očekáváme my.
#### [CIS.Core](index.md 'index')
### [CIS.Core.Exceptions.ExternalServices](CIS.Core.Exceptions.ExternalServices.md 'CIS.Core.Exceptions.ExternalServices')

## CisExtServiceValidationException Class

HTTP 400. Chyba, která vzniká při volání API třetích stran. Pokud API vrátí HTTP 4xx, vytáhneme z odpovědi chybu a vyvoláme tuto vyjímku. Podporuje seznam chyb.

```csharp
public sealed class CisExtServiceValidationException : CIS.Core.Exceptions.BaseCisValidationException
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; [System.Exception](https://docs.microsoft.com/en-us/dotnet/api/System.Exception 'System.Exception') &#129106; [BaseCisException](CIS.Core.Exceptions.BaseCisException.md 'CIS.Core.Exceptions.BaseCisException') &#129106; [BaseCisValidationException](CIS.Core.Exceptions.BaseCisValidationException.md 'CIS.Core.Exceptions.BaseCisValidationException') &#129106; CisExtServiceValidationException
### Constructors

<a name='CIS.Core.Exceptions.ExternalServices.CisExtServiceValidationException.CisExtServiceValidationException(string)'></a>

## CisExtServiceValidationException(string) Constructor

```csharp
public CisExtServiceValidationException(string message);
```
#### Parameters

<a name='CIS.Core.Exceptions.ExternalServices.CisExtServiceValidationException.CisExtServiceValidationException(string).message'></a>

`message` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Chybová zpráva
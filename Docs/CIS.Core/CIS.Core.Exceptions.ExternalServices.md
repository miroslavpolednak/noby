#### [CIS.Core](index.md 'index')

## CIS.Core.Exceptions.ExternalServices Namespace

| Classes | |
| :--- | :--- |
| [CisExtServiceResponseDeserializationException](CIS.Core.Exceptions.ExternalServices.CisExtServiceResponseDeserializationException.md 'CIS.Core.Exceptions.ExternalServices.CisExtServiceResponseDeserializationException') | Chyba, která vzniká při volání API třetích stran. Pokud se nepodaří deserializovat response volaného API, vyhodíme tuto vyjímku. |
| [CisExtServiceServerErrorException](CIS.Core.Exceptions.ExternalServices.CisExtServiceServerErrorException.md 'CIS.Core.Exceptions.ExternalServices.CisExtServiceServerErrorException') | |
| [CisExtServiceUnavailableException](CIS.Core.Exceptions.ExternalServices.CisExtServiceUnavailableException.md 'CIS.Core.Exceptions.ExternalServices.CisExtServiceUnavailableException') | Služba třetí strany (ExternalServices) není dostupná - např. špatné URL volané služby, nebo volaná služba vůbec neběží. |
| [CisExtServiceValidationException](CIS.Core.Exceptions.ExternalServices.CisExtServiceValidationException.md 'CIS.Core.Exceptions.ExternalServices.CisExtServiceValidationException') | HTTP 400. Chyba, která vzniká při volání API třetích stran. Pokud API vrátí HTTP 4xx, vytáhneme z odpovědi chybu a vyvoláme tuto vyjímku. Podporuje seznam chyb. |

# Error handling na FE API
Na **FE API** nepoužíváme standardní .NET error handling (*ProblemDetails*).
Požadavek je takový, aby se na FE vracelo pole chyb v tomto tvaru (jedná se o kolekci objektů `CIS.Infrastructure.WebApi.Types.ApiErrorItem`):
```
[
  { ErrorCode, Message, Severity },
  ...
]
```

Chyby - ať už standardní validační, systémové (500) nebo chyby z *Fluent Validation* - na FE posíláme formou *Exceptions*.
Vyjímky zachytáváme middlewarem `CIS.Infrastructure.WebApi.Middlewares.ApiExceptionMiddleware`.
Tento middleware zachytí vyhozenou vyjímku a vytvoří z ní HTTP response se status kódem podle typu dané chyby.

Překlad typů vyjímek na HTTP status kódy.
- `NotImplementedException`: HTTP 500
- `CisServiceUnavailableException`: HTTP 500
- `CisServiceServerErrorException`: HTTP 500
- `CisNotFoundException`: HTTP 404
- `CisConflictException`: HTTP 409
- `CisValidationException`: HTTP 400
- fallback: HTTP 500

## Anonymní uživatel
Zvláštním typem vyjímky je `AuthenticationException`.
Pokud není uživatel autentizován, vracíme HTTP status kód 401 se strukturou:
```
{
  RedirectUri: string
}
```
*RedirectUri* obsahuje adresu, na kterou má být uživatel přesměrován pro autentizaci - tj. CAAS.

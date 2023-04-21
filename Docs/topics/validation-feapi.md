# Validace requestů FE API
**FluentValidation** je napojena pomocí *MediatR.IPipelineBehavior* (`NOBY.Infrastructure.ErrorHandling.Internals.NobyValidationBehavior`).
Pipeline validuje všechny *MediatR* requesty.
Ve chvíli, kdy FluentValidation vrátí chybu/y, pipeline vyvolá vyjímku `NobyValidationException` obsahující seznam chyb z FluentValidation.

Vložení pipeline do *MediatR* probíhá během startupu aplikace extension metodou:

```csharp
builder.AddNobyServices();
```

## Error handling na FE API
Na **FE API** nepoužíváme standardní .NET error handling / response (*ProblemDetails*).
Požadavek je takový, aby se na FE vracelo pole chyb v tomto tvaru (jedná se o kolekci objektů `NOBY.Infrastructure.ErrorHandling.ApiErrorItem`):
```
[
  { ErrorCode, Message, Description, Severity },
  ...
]
```
`ErrorCode` je kód FE chyby, všechny error FE kódy jsou v číselné řadě **90xxx**. 
Error kódy by měli být definované v IT analýze pro každý konkrétní endpoint.
Pokud není definovaný konkrétní kód, použijeme sběrný kód **90001**.  
`Severity` je závažnost chyby (*Warning, Error*), která je definována IT analýzou.

Nestandardní (ne 90001) chyby jsou zapsány v tabulce na [tomto odkazu](https://wiki.kb.cz/display/HT/NOBY_FS_ErrorHandling).

Chyby - ať už standardní validační (400), systémové (500) nebo chyby z *Fluent Validation* - na FE posíláme formou *Exceptions*.
Validační chyby, které vzniknou přímo v handleru jsou typu `NOBY.Infrastructure.ErrorHandling.NobyValidationException`.
Vyjímky zachytáváme middlewarem `NOBY.Infrastructure.ErrorHandling.Internals.NobyApiExceptionMiddleware`.
Tento middleware zachytí vyhozenou vyjímku a vytvoří z ní HTTP response se status kódem podle typu dané chyby.

Překlad typů vyjímek na HTTP status kódy.
- `NotImplementedException`: HTTP 500
- `CisServiceUnavailableException`: HTTP 500
- `CisServiceServerErrorException`: HTTP 500
- `CisNotFoundException`: HTTP 404
- `CisConflictException`: HTTP 409
- `CisValidationException`: HTTP 400
- `NobyValidationException`: HTTP 400
- fallback: HTTP 500

## Anonymní uživatel
Zvláštním typem vyjímky je `AuthenticationException`.  
Pokud není uživatel autentizován, vracíme HTTP status kód 401 se strukturou `NOBY.Infrastructure.Security.ApiAuthenticationProblemDetail`:
```json
{
  "AuthenticationScheme": "string",
  "RedirectUri": "string"
}
```
`RedirectUri` obsahuje adresu, na kterou má být uživatel přesměrován pro autentizaci - tj. CAAS.  
`AuthenticationScheme` odkazuje na autentizační schéma, které je použité na této instanci aplikace.

## Příklad vrácení validační chyby z endpoint handleru
```csharp
public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
{
  ...
  // validace
  throw new NobyValidationException(90002);
  ...
}
```

## Obecný pattern pro evidenci a sdílení chybových kódů v FE API
Používáme podobný způsob jako v gRPC službách. 
Existuje kontajner obsahující všechny chyby a jejich popis - `NOBY.Infrastructure.ErrorHandling.ErrorCodeMapper`.
Do kolekce Messages tohoto kontajneru se přidávají nové custom chyby.

ErrorCodeMapper se inicializuje během startupu aplikace:
```csharp
...
ErrorCodeMapper.Init();
...
```

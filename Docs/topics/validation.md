# Validace requestu a byznys logiky
Obecně pro byznys validace incoming requestů používáme **FluentValidation** (https://docs.fluentvalidation.net/). 
Napojení FluentValidation je rozdílné pro gRPC a REST služby (viz. níže).

Pokud je potřeba dalších byznys validací dál v *MediatR* pipeline (tj. po validaci requestu - v handleru), vyvoláváme vyjímky z namespace **CIS.Exceptions** - hlavně `CisValidationException`.
`CisValidationException` je typ vyjímky, který označuje chybu v byznys validacích. Tento typ vyjímky také umožňuje zadání kolekce chyb s různými kódy.

Chyby v gRPC službách mají vždy kromě zprávy také číselný kód. Díky tomuto kódu může následně FE API reagovat na konkrétní chyby.
Každá služba / komponenta má přiřazený vlastní rozsah kód chyb.
Základní chyby definuje IT analytik, další chyby vyvolávané v aplikaci si definujeme sami. 
Seznam chyb a jejich kódy se evidují na Confluence zde https://wiki.kb.cz/display/HT/Error+code+list.
Zprávy všech vyjímek vyvolaných gRPC službami jsou vždy v angličtině.

## Univerzální validátory pro FluentValidation
V namespace `CIS.Infrastructure.WebApi.Validation` jsou definovány globální validátory. 
Tyto je možné používat v custom validacích + je možné přidat další v případě potřeby.

Aktuálně jsou to:
- [NullObjectModelValidator](../../CIS/WebApi/Validation/NullObjectModelValidator.cs)
- [PaginationRequestValidator](../../CIS/WebApi/Validation/PaginationRequestValidator.cs)

## Implementace validace
[Implementace validace v gRPC službách](./validation-grpc.md)

[Implementace validace v FE API](./validation-feapi.md)

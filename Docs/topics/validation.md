# Validace
Obecně pro byznys validace incoming requestů používáme **FluentValidation** (https://docs.fluentvalidation.net/). 
Napojení FluentValidation je rozdílné pro gRPC a REST služby.

Pokud je potřeba dalších byznys validací dál v *MediatR* pipeline (tj. v handleru), vyvoláváme vyjímky z namespace **CIS.Exceptions** - hlavně `CisValidationException`.

## Validace requestů gRPC služeb
FluentValidation je napojena pomocí *MediatR.IPipelineBehavior* (`CIS.Infrastructure.gRPC.Validation.GrpcValidationBehaviour`).
Pipeline validuje všechny *MediatR* requesty, které implementují marker interface `CIS.Core.Validation.IValidatableRequest`.
Ve chvíli, kdy FluentValidation vrátí chybu/y, pipeline vyvolá vyjímku `CisValidationException` obsahující seznam chyb z FluentValidation.

Vložení pipeline do *MediatR* probíhá během startupu aplikace extension metodou:

```
builder.Services.AddCisGrpcInfrastructure(typeof(Program));
```

Dále je nutné zaregistrovat gRPC interceptor, který zachytí takto vyvolané vyjímky a zkonvertuje je na `RpcException` různých typů tak, aby Clients projekty volajících služeb mohli zase zpětně zrekonstruovat původní vyjímku.
Výchozí exception interceptor je `CIS.Infrastructure.gRPC.GenericServerExceptionInterceptor`.
Zároveň je možné do gRPC pipeline přidat další custom exception interceptory, pokud by tento výchozí nestačil.

## Validace requestů REST služeb / FE API
FluentValidation se do *MVC* pipeline vloží standardním způsobem během startupu extension metodou:

```
builder.Services.AddControllers().AddFluentValidation();
```

Narozdíl od gRPC implementace se validuje request přicházející přímo do endpointu, nikoliv *MediatR* request.

## Univerzální validátory pro FluentValidation
V namespace `CIS.Infrastructure.WebApi.Validation` jsou definovány globální validátory. 
Tyto je možné používat v custom validacích + je možné přidat další v případě potřeby.

Aktuálně jsou to:
- [NullObjectModelValidator](../../CIS/WebApi/Validation/NullObjectModelValidator.cs)
- [PaginationRequestValidator](../../CIS/WebApi/Validation/PaginationRequestValidator.cs)

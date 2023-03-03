# Validace requestů gRPC služeb
FluentValidation je napojena pomocí *MediatR.IPipelineBehavior* (`CIS.Infrastructure.gRPC.Validation.GrpcValidationBehaviour`).
Pipeline validuje všechny *MediatR* requesty, které implementují marker interface `CIS.Core.Validation.IValidatableRequest`.
Ve chvíli, kdy FluentValidation vrátí chybu/y, pipeline vyvolá vyjímku `CisValidationException` obsahující seznam chyb z FluentValidation.

Vložení pipeline do *MediatR* probíhá během startupu aplikace extension metodou:

```csharp
builder.Services.AddCisGrpcInfrastructure(typeof(Program));
```

Dále je nutné zaregistrovat gRPC interceptor, který zachytí takto vyvolané vyjímky a zkonvertuje je na `RpcException` různých typů tak, aby Clients projekty volajících služeb mohli zase zpětně zrekonstruovat původní vyjímku.
Výchozí exception interceptor je `CIS.Infrastructure.gRPC.GenericServerExceptionInterceptor`.
Zároveň je možné do gRPC pipeline přidat další custom exception interceptory, pokud by tento výchozí nestačil.

## Obecný pattern pro evidenci a sdílení chybových kódů v gRPC službách
Chceme, aby všechny chybové kódy v dané službě byly uloženy jako konstanty na jednom místě. 
Zároveň na stejném místě musí být uložené i textové popisy patřící k jednotlivým chybovým kódům.

Abychom tohoto docílili, platí konvence, že v rootu *Api* projektu existuje třída `ErrorCodeMapper`, která slouží jako zdroj těchto dat.
Tato třída vždy dědí z abstraktní třídy `CIS.Core.ErrorCodes.ErrorCodeMapperBase`.  
`ErrorCodeMapper` obsahuje:
1) Konstanty reprezentující chybové kódy.
Jedná se o `Int32` se jménem vyjadřujícím typ vyjímky, jehož hodnota je mapována na error kódy na Confluence.
2) Překladový slovník z kódu chyby na její textový popis.
Textový popis může obsahovat placeholder `{PropertyValue}`, který je v pipeline *FluentValidation* nahrazen aktuální hodnotou v kontrolované vlastnosti.  
Slovník chyb je dále dostupný jako statická vlastnost `Messages`.

> Cílově se nesmí stát, že v aplikaci bude existovat vyjímka, která má v konstruktoru zadaný přímo vlastní kód chyby - vždy tento kód musí být zadán konstantou z `ErrorCodeMapper `.

Ukázka `ErrorCodeMapper` třídy v *Api* projektu:
```csharp
internal sealed class ErrorCodeMapper : CIS.Core.ErrorCodes.ErrorCodeMapperBase
{
    public const int CustomerOnSANotFound = 16020;
    ...

    public static IErrorCodesDictionary Init()
    {
        SetMessages(new Dictionary<int, string>()
        {
            { CustomerOnSANotFound , "Customer ID {PropertyValue} not found" },
            ...
        });

        return Messages;
    }
}
```

## Vyvolání vyjímek v aplikaci s použitím slovníku chybových textů
```csharp
// Klasická vyjímka.
// Chybový kód je v konstantě ErrorCodeMapper.ObligationNotFound.
// Text chyby se načte helper metodou GetMessage() z base třídy.
throw new CisNotFoundException(ErrorCodeMapper.ObligationNotFound, ErrorCodeMapper.GetMessage(ErrorCodeMapper.ObligationNotFound));

// Helper v base třídě pro jednodušší vytvoření nové vyjímky.
throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.ObligationNotFound);
// Helper s použití přetížení, kdy je řetězec {PropertyValue} v textu nahrazenou hodnotou request.ObligationId.
throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.ObligationNotFound, request.ObligationId);
```

## Texty a chybové kódy ve FluentValidation
Aby *FluentValidation* používalo tyto custom texty pro naše chybové kódy, používáme možnost vložit do *FluentValidation* vlastní [**LanguageManager**](https://docs.fluentvalidation.net/en/latest/localization.html).
Tím nahradíme výchozí texty v *FluentValidation* naší kolekcí `ErrorCodeMapper.Messages`.
Nastavení *LanguageManager* probíhá automaticky při startupu aplikace pomocí extension metody `AddCisGrpcInfrastructure()`, kdy je jako druhý parametr předán slovník chybových textů.
```csharp
builder.Services.AddCisGrpcInfrastructure(typeof(Program), ErrorCodeMapper.Init());
```

### Nové extension metody ve validátorech
Nové extension metody pro *FluentValidation* se nacházejí v namespace `CIS.Infrastructure.CisMediatR.GrpcValidation`.

Ve validátorech pak používáme extension metodu `WithErrorCode()`.
Tato metoda přijímá `Int32` - konstantu z `ErrorCodeMapper` - a promapuje textový popis na kód ze slovníku `Messages`.
```csharp
RuleFor(t => t.CustomerOnSAId)
    .GreaterThan(0)
    .WithErrorCode(ErrorCodeMapper.CustomerOnSANotFound);
```

Ve validátorech je také možnost použít extension metodu `ThrowCisException()`, která umožňuje při chybné validaci vyvolat jiný typ vyjímky než `CisValidationException`.
To je užitečné např. při kontrole na existenci objektu, kdy potřebujeme vyvolat vyjímku `CisNotFoundException` (HTTP 404).
```csharp
RuleFor(t => t.CustomerOnSAId)
    .MustAsync(async (customerOnSAId, cancellationToken) => await dbContext.Customers.AnyAsync(t => t.CustomerOnSAId == customerOnSAId, cancellationToken))
    .WithErrorCode(ErrorCodeMapper.CustomerOnSANotFound)
    .ThrowCisException(GrpcValidationBehaviorExceptionTypes.CisNotFoundException);
```

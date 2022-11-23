# Validace
Obecně pro byznys validace incoming requestů používáme **FluentValidation** (https://docs.fluentvalidation.net/). 
Napojení FluentValidation je rozdílné pro gRPC a REST služby.

Pokud je potřeba dalších byznys validací v MediatR handleru, vyvoláváme vyjímky z namespace CIS.Exceptions - hlavně CisValidationException. 

## Validace requestů gRPC služeb
FluentValidation je napojena pomocí *MediatR.IPipelineBehavior* (CIS.Infrastructure.gRPC.Validation.**GrpcValidationBehaviour**).
Pipeline validuje všechny MediatR requesty, které implementují marker interface CIS.Core.Validation.**IValidatableRequest**.
Ve chvíli, kdy FluentValidation vrátí chybu/y, pipeline vyvolá vyjímku *CisValidationException*.

## Validace requestů REST služeb / FE API


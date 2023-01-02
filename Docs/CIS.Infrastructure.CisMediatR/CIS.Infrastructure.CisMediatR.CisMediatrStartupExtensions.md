#### [CIS.Infrastructure.CisMediatR](index.md 'index')
### [CIS.Infrastructure.CisMediatR](CIS.Infrastructure.CisMediatR.md 'CIS.Infrastructure.CisMediatR')

## CisMediatrStartupExtensions Class

Extension metody do startupu aplikace pro registraci behaviors.

```csharp
public static class CisMediatrStartupExtensions
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; CisMediatrStartupExtensions
### Methods

<a name='CIS.Infrastructure.CisMediatR.CisMediatrStartupExtensions.AddCisMediatrRollbackCapability(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection)'></a>

## CisMediatrStartupExtensions.AddCisMediatrRollbackCapability(this IServiceCollection) Method

Pridava moznost rollbacku do Mediatr handleru. Rollback se spusti vyhozenim exception kdykoliv v prubehu exekuce handleru. Po ukonceni rollbacku se dana exception propaguje standardne dal do pipeline.

```csharp
public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddCisMediatrRollbackCapability(this Microsoft.Extensions.DependencyInjection.IServiceCollection services);
```
#### Parameters

<a name='CIS.Infrastructure.CisMediatR.CisMediatrStartupExtensions.AddCisMediatrRollbackCapability(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection).services'></a>

`services` [Microsoft.Extensions.DependencyInjection.IServiceCollection](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.DependencyInjection.IServiceCollection 'Microsoft.Extensions.DependencyInjection.IServiceCollection')

#### Returns
[Microsoft.Extensions.DependencyInjection.IServiceCollection](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.DependencyInjection.IServiceCollection 'Microsoft.Extensions.DependencyInjection.IServiceCollection')

### Remarks
Moznost rollbacku se do Mediatr Requestu prida dedenim interface [IRollbackCapable](CIS.Infrastructure.CisMediatR.Rollback.IRollbackCapable.md 'CIS.Infrastructure.CisMediatR.Rollback.IRollbackCapable'), napr. class MyRequest : IRequest<T>, IRollbackCapable {}  
Dale je nutne vytvorit vlastni kod rollbacku. To je trida dedici z [IRollbackAction](CIS.Infrastructure.CisMediatR.Rollback.IRollbackAction_TRequest_.md 'CIS.Infrastructure.CisMediatR.Rollback.IRollbackAction<TRequest>') - vlozeni teto tridy do DI je v gesci volajici aplikace.  
Pro prenos dat mezi Mediatr handlerem a rollback akci je pouzita scoped instance [IRollbackBag](CIS.Infrastructure.CisMediatR.Rollback.IRollbackBag.md 'CIS.Infrastructure.CisMediatR.Rollback.IRollbackBag'). Do teto instance by mel handler postupne ukladat metadata potrebna pro rollback (napr. vytvorena Id entit).
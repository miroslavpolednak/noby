#### [CIS.Core](index.md 'index')
### [CIS.Core](CIS.Core.md 'CIS.Core')

## ServiceCollectionExtensions Class

```csharp
public static class ServiceCollectionExtensions
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; ServiceCollectionExtensions
### Methods

<a name='CIS.Core.ServiceCollectionExtensions.AlreadyRegistered_TService_(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection)'></a>

## ServiceCollectionExtensions.AlreadyRegistered<TService>(this IServiceCollection) Method

Zjisteni, zda je dany typ jiz registrovan v DI

```csharp
public static bool AlreadyRegistered<TService>(this Microsoft.Extensions.DependencyInjection.IServiceCollection services);
```
#### Type parameters

<a name='CIS.Core.ServiceCollectionExtensions.AlreadyRegistered_TService_(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection).TService'></a>

`TService`
#### Parameters

<a name='CIS.Core.ServiceCollectionExtensions.AlreadyRegistered_TService_(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection).services'></a>

`services` [Microsoft.Extensions.DependencyInjection.IServiceCollection](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.DependencyInjection.IServiceCollection 'Microsoft.Extensions.DependencyInjection.IServiceCollection')

#### Returns
[System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')
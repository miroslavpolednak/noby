#### [CIS.Infrastructure.gRPC](index.md 'index')
### [CIS.Infrastructure.gRPC](CIS.Infrastructure.gRPC.md 'CIS.Infrastructure.gRPC')

## GrpcStartupExtensions Class

Extension metody do startupu aplikace pro registraci gRPC služeb.

```csharp
public static class GrpcStartupExtensions
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; GrpcStartupExtensions
### Methods

<a name='CIS.Infrastructure.gRPC.GrpcStartupExtensions.AddCisGrpcInfrastructure(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,System.Type)'></a>

## GrpcStartupExtensions.AddCisGrpcInfrastructure(this IServiceCollection, Type) Method

Zaregistruje do DI:  
- MediatR  
- FluentValidation through MediatR pipelines

```csharp
public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddCisGrpcInfrastructure(this Microsoft.Extensions.DependencyInjection.IServiceCollection services, System.Type assemblyType);
```
#### Parameters

<a name='CIS.Infrastructure.gRPC.GrpcStartupExtensions.AddCisGrpcInfrastructure(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,System.Type).services'></a>

`services` [Microsoft.Extensions.DependencyInjection.IServiceCollection](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.DependencyInjection.IServiceCollection 'Microsoft.Extensions.DependencyInjection.IServiceCollection')

<a name='CIS.Infrastructure.gRPC.GrpcStartupExtensions.AddCisGrpcInfrastructure(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,System.Type).assemblyType'></a>

`assemblyType` [System.Type](https://docs.microsoft.com/en-us/dotnet/api/System.Type 'System.Type')

Typ, který je v hlavním projektu - typicky Program.cs

#### Returns
[Microsoft.Extensions.DependencyInjection.IServiceCollection](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.DependencyInjection.IServiceCollection 'Microsoft.Extensions.DependencyInjection.IServiceCollection')
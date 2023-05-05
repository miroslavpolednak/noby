#### [CIS.Infrastructure.gRPC](index.md 'index')
### [CIS.Infrastructure.gRPC](CIS.Infrastructure.gRPC.md 'CIS.Infrastructure.gRPC')

## StartupExtensions Class

Extension metody do startupu aplikace pro registraci gRPC služeb.

```csharp
public static class StartupExtensions
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; StartupExtensions
### Methods

<a name='CIS.Infrastructure.gRPC.StartupExtensions.AddCisGrpcClientInner_TService,TServiceUriSettings_(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,bool,bool)'></a>

## StartupExtensions.AddCisGrpcClientInner<TService,TServiceUriSettings>(this IServiceCollection, bool, bool) Method

Nepouzivat primo, je public pouze pro ServiceDiscovery nebo jine specialni pripady.

```csharp
public static Microsoft.Extensions.DependencyInjection.IHttpClientBuilder? AddCisGrpcClientInner<TService,TServiceUriSettings>(this Microsoft.Extensions.DependencyInjection.IServiceCollection services, bool validateServiceCertificate, bool forwardClientHeaders)
    where TService : class
    where TServiceUriSettings : class;
```
#### Type parameters

<a name='CIS.Infrastructure.gRPC.StartupExtensions.AddCisGrpcClientInner_TService,TServiceUriSettings_(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,bool,bool).TService'></a>

`TService`

<a name='CIS.Infrastructure.gRPC.StartupExtensions.AddCisGrpcClientInner_TService,TServiceUriSettings_(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,bool,bool).TServiceUriSettings'></a>

`TServiceUriSettings`
#### Parameters

<a name='CIS.Infrastructure.gRPC.StartupExtensions.AddCisGrpcClientInner_TService,TServiceUriSettings_(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,bool,bool).services'></a>

`services` [Microsoft.Extensions.DependencyInjection.IServiceCollection](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.DependencyInjection.IServiceCollection 'Microsoft.Extensions.DependencyInjection.IServiceCollection')

<a name='CIS.Infrastructure.gRPC.StartupExtensions.AddCisGrpcClientInner_TService,TServiceUriSettings_(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,bool,bool).validateServiceCertificate'></a>

`validateServiceCertificate` [System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')

<a name='CIS.Infrastructure.gRPC.StartupExtensions.AddCisGrpcClientInner_TService,TServiceUriSettings_(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,bool,bool).forwardClientHeaders'></a>

`forwardClientHeaders` [System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')

#### Returns
[Microsoft.Extensions.DependencyInjection.IHttpClientBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.DependencyInjection.IHttpClientBuilder 'Microsoft.Extensions.DependencyInjection.IHttpClientBuilder')

<a name='CIS.Infrastructure.gRPC.StartupExtensions.AddCisGrpcInfrastructure(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,System.Type,CIS.Core.ErrorCodes.IErrorCodesDictionary)'></a>

## StartupExtensions.AddCisGrpcInfrastructure(this IServiceCollection, Type, IErrorCodesDictionary) Method

Zaregistruje do DI:  
- MediatR  
- FluentValidation through MediatR pipelines

```csharp
public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddCisGrpcInfrastructure(this Microsoft.Extensions.DependencyInjection.IServiceCollection services, System.Type assemblyType, CIS.Core.ErrorCodes.IErrorCodesDictionary? validationMessages=null);
```
#### Parameters

<a name='CIS.Infrastructure.gRPC.StartupExtensions.AddCisGrpcInfrastructure(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,System.Type,CIS.Core.ErrorCodes.IErrorCodesDictionary).services'></a>

`services` [Microsoft.Extensions.DependencyInjection.IServiceCollection](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.DependencyInjection.IServiceCollection 'Microsoft.Extensions.DependencyInjection.IServiceCollection')

<a name='CIS.Infrastructure.gRPC.StartupExtensions.AddCisGrpcInfrastructure(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,System.Type,CIS.Core.ErrorCodes.IErrorCodesDictionary).assemblyType'></a>

`assemblyType` [System.Type](https://docs.microsoft.com/en-us/dotnet/api/System.Type 'System.Type')

Typ, který je v hlavním projektu - typicky Program.cs

<a name='CIS.Infrastructure.gRPC.StartupExtensions.AddCisGrpcInfrastructure(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,System.Type,CIS.Core.ErrorCodes.IErrorCodesDictionary).validationMessages'></a>

`validationMessages` [CIS.Core.ErrorCodes.IErrorCodesDictionary](https://docs.microsoft.com/en-us/dotnet/api/CIS.Core.ErrorCodes.IErrorCodesDictionary 'CIS.Core.ErrorCodes.IErrorCodesDictionary')

Slovník pro překládání chybových kódů ve FluentValidation na naše error messages. [ExceptionCode, Message]

#### Returns
[Microsoft.Extensions.DependencyInjection.IServiceCollection](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.DependencyInjection.IServiceCollection 'Microsoft.Extensions.DependencyInjection.IServiceCollection')
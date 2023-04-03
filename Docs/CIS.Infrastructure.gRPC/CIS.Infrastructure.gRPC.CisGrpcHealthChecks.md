#### [CIS.Infrastructure.gRPC](index.md 'index')
### [CIS.Infrastructure.gRPC](CIS.Infrastructure.gRPC.md 'CIS.Infrastructure.gRPC')

## CisGrpcHealthChecks Class

```csharp
public static class CisGrpcHealthChecks
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; CisGrpcHealthChecks
### Methods

<a name='CIS.Infrastructure.gRPC.CisGrpcHealthChecks.AddCisGrpcHealthChecks(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder)'></a>

## CisGrpcHealthChecks.AddCisGrpcHealthChecks(this WebApplicationBuilder) Method

Zaregistruje health checky pro gRPC sluzby + registruje healthcheck i pro HTTP1.1.

```csharp
public static Microsoft.Extensions.DependencyInjection.IHealthChecksBuilder AddCisGrpcHealthChecks(this Microsoft.AspNetCore.Builder.WebApplicationBuilder builder);
```
#### Parameters

<a name='CIS.Infrastructure.gRPC.CisGrpcHealthChecks.AddCisGrpcHealthChecks(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder).builder'></a>

`builder` [Microsoft.AspNetCore.Builder.WebApplicationBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.Builder.WebApplicationBuilder 'Microsoft.AspNetCore.Builder.WebApplicationBuilder')

#### Returns
[Microsoft.Extensions.DependencyInjection.IHealthChecksBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.DependencyInjection.IHealthChecksBuilder 'Microsoft.Extensions.DependencyInjection.IHealthChecksBuilder')

### Remarks
Pridava healthcheck na sluzbu jako takovou a databaze v ni pouzite.

<a name='CIS.Infrastructure.gRPC.CisGrpcHealthChecks.MapCisGrpcHealthChecks(thisMicrosoft.AspNetCore.Routing.IEndpointRouteBuilder)'></a>

## CisGrpcHealthChecks.MapCisGrpcHealthChecks(this IEndpointRouteBuilder) Method

Mapuje gRPC a HTTP1.1 healthcheck endpoint.

```csharp
public static Microsoft.AspNetCore.Routing.IEndpointRouteBuilder MapCisGrpcHealthChecks(this Microsoft.AspNetCore.Routing.IEndpointRouteBuilder app);
```
#### Parameters

<a name='CIS.Infrastructure.gRPC.CisGrpcHealthChecks.MapCisGrpcHealthChecks(thisMicrosoft.AspNetCore.Routing.IEndpointRouteBuilder).app'></a>

`app` [Microsoft.AspNetCore.Routing.IEndpointRouteBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.Routing.IEndpointRouteBuilder 'Microsoft.AspNetCore.Routing.IEndpointRouteBuilder')

#### Returns
[Microsoft.AspNetCore.Routing.IEndpointRouteBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.Routing.IEndpointRouteBuilder 'Microsoft.AspNetCore.Routing.IEndpointRouteBuilder')
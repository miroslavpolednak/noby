#### [CIS.Infrastructure.gRPC](index.md 'index')
### [CIS.Infrastructure.gRPC.Validation](CIS.Infrastructure.gRPC.Validation.md 'CIS.Infrastructure.gRPC.Validation')

## GrpcValidationBehaviour<TRequest,TResponse> Class

MediatR pipeline, která přidává do flow requestu FluentValidation.

```csharp
public sealed class GrpcValidationBehaviour<TRequest,TResponse> :
MediatR.IPipelineBehavior<TRequest, TResponse>
    where TRequest : MediatR.IRequest<TResponse>, CIS.Core.Validation.IValidatableRequest
```
#### Type parameters

<a name='CIS.Infrastructure.gRPC.Validation.GrpcValidationBehaviour_TRequest,TResponse_.TRequest'></a>

`TRequest`

<a name='CIS.Infrastructure.gRPC.Validation.GrpcValidationBehaviour_TRequest,TResponse_.TResponse'></a>

`TResponse`

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; GrpcValidationBehaviour<TRequest,TResponse>

Implements [MediatR.IPipelineBehavior&lt;](https://docs.microsoft.com/en-us/dotnet/api/MediatR.IPipelineBehavior-2 'MediatR.IPipelineBehavior`2')[TRequest](CIS.Infrastructure.gRPC.Validation.GrpcValidationBehaviour_TRequest,TResponse_.md#CIS.Infrastructure.gRPC.Validation.GrpcValidationBehaviour_TRequest,TResponse_.TRequest 'CIS.Infrastructure.gRPC.Validation.GrpcValidationBehaviour<TRequest,TResponse>.TRequest')[,](https://docs.microsoft.com/en-us/dotnet/api/MediatR.IPipelineBehavior-2 'MediatR.IPipelineBehavior`2')[TResponse](CIS.Infrastructure.gRPC.Validation.GrpcValidationBehaviour_TRequest,TResponse_.md#CIS.Infrastructure.gRPC.Validation.GrpcValidationBehaviour_TRequest,TResponse_.TResponse 'CIS.Infrastructure.gRPC.Validation.GrpcValidationBehaviour<TRequest,TResponse>.TResponse')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/MediatR.IPipelineBehavior-2 'MediatR.IPipelineBehavior`2')

### Remarks
Pokud v rámci pipeline handleru vrátí FluentValidation chyby, vyhodíme vyjímku CisValidationException a ukončí se flow requestu.
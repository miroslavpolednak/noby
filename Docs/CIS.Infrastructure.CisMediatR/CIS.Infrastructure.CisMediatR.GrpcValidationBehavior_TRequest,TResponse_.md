#### [CIS.Infrastructure.CisMediatR](index.md 'index')
### [CIS.Infrastructure.CisMediatR](CIS.Infrastructure.CisMediatR.md 'CIS.Infrastructure.CisMediatR')

## GrpcValidationBehavior<TRequest,TResponse> Class

MediatR pipeline, která přidává do flow requestu FluentValidation.

```csharp
public sealed class GrpcValidationBehavior<TRequest,TResponse> :
MediatR.IPipelineBehavior<TRequest, TResponse>
    where TRequest : MediatR.IRequest<TResponse>, CIS.Core.Validation.IValidatableRequest
```
#### Type parameters

<a name='CIS.Infrastructure.CisMediatR.GrpcValidationBehavior_TRequest,TResponse_.TRequest'></a>

`TRequest`

<a name='CIS.Infrastructure.CisMediatR.GrpcValidationBehavior_TRequest,TResponse_.TResponse'></a>

`TResponse`

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; GrpcValidationBehavior<TRequest,TResponse>

Implements [MediatR.IPipelineBehavior&lt;](https://docs.microsoft.com/en-us/dotnet/api/MediatR.IPipelineBehavior-2 'MediatR.IPipelineBehavior`2')[TRequest](CIS.Infrastructure.CisMediatR.GrpcValidationBehavior_TRequest,TResponse_.md#CIS.Infrastructure.CisMediatR.GrpcValidationBehavior_TRequest,TResponse_.TRequest 'CIS.Infrastructure.CisMediatR.GrpcValidationBehavior<TRequest,TResponse>.TRequest')[,](https://docs.microsoft.com/en-us/dotnet/api/MediatR.IPipelineBehavior-2 'MediatR.IPipelineBehavior`2')[TResponse](CIS.Infrastructure.CisMediatR.GrpcValidationBehavior_TRequest,TResponse_.md#CIS.Infrastructure.CisMediatR.GrpcValidationBehavior_TRequest,TResponse_.TResponse 'CIS.Infrastructure.CisMediatR.GrpcValidationBehavior<TRequest,TResponse>.TResponse')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/MediatR.IPipelineBehavior-2 'MediatR.IPipelineBehavior`2')

### Remarks
Pokud v rámci pipeline handleru vrátí FluentValidation chyby, vyhodíme vyjímku CisValidationException a ukončí se flow requestu.